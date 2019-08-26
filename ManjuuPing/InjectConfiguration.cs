using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ManjuuDomain.IDomain;
using NLog;
using NLog.Extensions.Logging;

namespace ManjuuPing
{
    /// <summary>
    /// 类型注入
    /// </summary>
    public class InjectConfiguration
    {
        public static IContainer Container{get; private set;}

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


    }
}