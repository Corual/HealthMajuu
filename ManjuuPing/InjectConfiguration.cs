using System;
using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ManjuuCommon.ILog;
using ManjuuDomain.IDomain;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Extensions.Logging;
using System.Linq;
using ManjuuCommon.ILog.NLog;

namespace ManjuuPing
{
    /// <summary>
    /// 类型注入
    /// </summary>
    public class InjectConfiguration
    {
        public static IContainer Container { get; private set; }

        //public static ServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// 配置DI
        /// </summary>
        public static void DeployAutoFac()
        {
            //Inversion of Control 控制反转 (创建IOC容器构建对象 )
            var builder = new ContainerBuilder();

            //将相关程序集实现类注册到容器中
            builder.RegisterAssemblyTypes(Assembly.Load("ManjuuDomain"), Assembly.Load("ManjuuInfrastructure"))
            .Where(p => !p.IsAbstract && typeof(IRepository).IsAssignableFrom(p))
            .AsImplementedInterfaces().PropertiesAutowired().SingleInstance();


            //var types = Assembly.Load("ManjuuCommon").GetTypes().Where(p => !p.IsAbstract && typeof(ICustomLog<>).IsAssignableFrom(p));

            //if (null != types && types.Any())
            //{
            //    foreach (var item in types)
            //    {
            //        builder.RegisterGeneric(item).AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            //    }
            //}


            builder.RegisterAssemblyTypes(Assembly.Load("ManjuuCommon"))
            .Where(p => !p.IsAbstract && typeof(ICustomLog<ILogger>).IsAssignableFrom(p))
            .AsImplementedInterfaces().PropertiesAutowired().SingleInstance();

            NLogMgr.SetVariable(NLogMgr.ConfigurationVariables.Terrace, "检测工具");

            Container = builder.Build();

        }

        //public static void DeployNLog()
        //{
        //    

        //    //读取微软的日志接口配置
        //    IConfiguration config = new ConfigurationBuilder()
        //     .SetBasePath(Path.GetDirectoryName(typeof(InjectConfiguration).Assembly.Location)) //From NuGet Package Microsoft.Extensions.Configuration.Json
        //     .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //     .Build();

        //    ServiceProvider = new ServiceCollection()
        //        //.AddTransient<TestNLog>() // 这里开始注入配置
        //       .AddLogging(loggingBuilder =>
        //       {
        //  // 配置NLog
        //  loggingBuilder.ClearProviders();
        //           loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
        //           loggingBuilder.AddNLog(config);
        //       })
        //       .BuildServiceProvider();

            
        //}


    }
}