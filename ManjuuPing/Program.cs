using JKang.IpcServiceFramework;
using ManjuuApplications;
using ManjuuCommon.ILog;
using ManjuuCommon.ILog.NLog;
using ManjuuDomain.IDomain;
using ManjuuInfrastructure.IpcService.ServiceContract;
using ManjuuPing.IpcServerImp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ManjuuPing
{
    class Program
    {
        public static async Task Main(string[] args)
        {

            var host = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    #region Host环境设置
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("hostsettings.json", optional: true);
                    configHost.AddEnvironmentVariables(prefix: "PREFIX_");
                    configHost.AddCommandLine(args);
                    #endregion
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    #region 程序配置
                    configApp.AddJsonFile("appsettings.json", optional: true);
                    configApp.AddJsonFile(
                        $"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
                        optional: true);
                    configApp.AddEnvironmentVariables(prefix: "PREFIX_");
                    configApp.AddCommandLine(args);
                    #endregion
                })
                .ConfigureServices((hostContext, services) =>
                {

                    //程序都在这里注入

                    #region IRepository
                    var infrastructureAssembly = Assembly.Load("ManjuuInfrastructure");
                    var repositoryTypes = infrastructureAssembly.GetTypes().Where(p => !p.IsAbstract && typeof(IRepository).IsAssignableFrom(p));
                    foreach (var item in repositoryTypes)
                    {
                        foreach (var itemIntface in item.GetInterfaces())
                        {
                            if (typeof(IRepository) == itemIntface) { continue; }
                            services.AddSingleton(itemIntface, item);
                        }
                    }
                    #endregion

                    #region ICustomLog
                    var commonAssembly = Assembly.Load("ManjuuCommon");
                    var customLogTypes = commonAssembly.GetTypes().Where(p => !p.IsAbstract && typeof(ICustomLog<NLog.ILogger>).IsAssignableFrom(p));
                    foreach (var item in customLogTypes)
                    {
                        foreach (var itemIntface in item.GetInterfaces())
                        {
                            if (typeof(ICustomLog<NLog.ILogger>) == itemIntface) { continue; }
                            services.AddSingleton(itemIntface, item);
                        }
                    }
                    #endregion

                    #region IApplication
                    var applicationAssembly = Assembly.Load("ManjuuApplications");
                    var applicationTypes = applicationAssembly.GetTypes().Where(p => !p.IsAbstract && typeof(IApplication).IsAssignableFrom(p));
                    foreach (var item in applicationTypes)
                    {
                        foreach (var itemIntface in item.GetInterfaces())
                        {
                            if (typeof(IApplication) == itemIntface) { continue; }
                            //同一个请求下单例
                            services.AddScoped(itemIntface, item);
                        }
                    }

                    #endregion

                    #region Quartz.Net
                    services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();//注册ISchedulerFactory的实例。
                    #endregion

                    #region Ipc
                    services.AddLogging(l =>
                    {
                        l.AddConsole();
                        l.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);
                    })
                    .AddIpc(action =>
                    {
                        action.AddNamedPipe(pipeOpt =>
                        {
                            pipeOpt.ThreadCount = 2;
                        }).AddService<IDemoServiceContract, DemoServiceContract>()
                        .AddService<ICheckConfigServiceContract, PingServiceContract>()
                        .AddService<ICheckTargetServiceContract, PingServiceContract>();

                    });


                    #region Server
                    var ipcServerHost = new IpcServiceHostBuilder(services.BuildServiceProvider())
                                    //.AddNamedPipeEndpoint<IDemoServiceContract>("demoEnpoint", "pipeName")
                                    .AddNamedPipeEndpoint<ICheckConfigServiceContract>("ConfigServiceEnpoint", "configPipe")
                                    .AddNamedPipeEndpoint<ICheckTargetServiceContract>("TargetServiceEnpoint", "targetPipe")
                                    //.AddTcpEndpoint<IDemoServiceContract>("demoTcpEndpoiint", IPAddress.Loopback, 2324)
                                    .Build();

                    services.AddSingleton<IIpcServiceHost>(ipcServerHost);
                    #endregion

                    #region Client
                    IpcServiceClient<ICheckConfigServiceContract> configClient = new IpcServiceClientBuilder<ICheckConfigServiceContract>()
                   .UseNamedPipe("configPipe") // or .UseTcp(IPAddress.Loopback, 45684) to invoke using TCP 
                   .Build();
                    IpcServiceClient<ICheckTargetServiceContract> targetJobClient = new IpcServiceClientBuilder<ICheckTargetServiceContract>()
                 .UseNamedPipe("targetPipe")
                 .Build();

                    services.AddSingleton<IpcServiceClient<ICheckConfigServiceContract>>(configClient);

                    services.AddSingleton<IpcServiceClient<ICheckTargetServiceContract>>(targetJobClient);
                    #endregion

                    #endregion

                    NLogMgr.SetVariable(NLogMgr.ConfigurationVariables.Terrace, "检测工具");

                    services.AddHostedService<PingHostedService>();


                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    //日志中间件
                    //configLogging.AddConsole();
                    //configLogging.AddDebug();
                })
                .UseConsoleLifetime()
                .Build();

            await host.RunAsync();
        }
    }
}
