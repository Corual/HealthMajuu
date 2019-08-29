using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManjuuCommon.Tools
{
    public static class ServiceLaunchHelper
    {
        public static string GetStartEndpoint(string[] args)
        {
            string addressString = string.Empty;

            //创建配置builder,获取自定义启动地址与端口
            var config = new ConfigurationBuilder()
                .AddCommandLine(args) //添加启动是传入的参数
                .Build();

            string ip = config["ip"]; //ip地址
            string port = config["port"]; //要占用的端口号

            if (string.IsNullOrEmpty(ip) && string.IsNullOrEmpty(port))
            {
                return string.Empty;
            }

            ip = ip ?? "localhost";
            port = port ?? "5000";
            addressString = string.Format($"http://{ip}:{port}");

            return addressString;


        }
    }
}
