using System;
using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ManjuuDomain.IDomain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;

namespace ManjuuPing
{
    /// <summary>
    /// 类型注入
    /// </summary>
    public class InjectConfiguration
    {
        public static IContainer Container { get; private set; }

        public static ServiceProvider ServiceProvider { get; private set; }

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


            Container = builder.Build();

        }

        public static void DeployNLog()
        {
            //读取微软的日志接口配置
            IConfiguration config = new ConfigurationBuilder()
             .SetBasePath(Path.GetDirectoryName(typeof(InjectConfiguration).Assembly.Location)) //From NuGet Package Microsoft.Extensions.Configuration.Json
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
             .Build();

            ServiceProvider = new ServiceCollection()
               .AddTransient<TestNLog>() // 这里开始注入配置
               .AddLogging(loggingBuilder =>
               {
          // 配置NLog
          loggingBuilder.ClearProviders();
                   loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                   loggingBuilder.AddNLog(config);
               })
               .BuildServiceProvider();
        }


    }
}