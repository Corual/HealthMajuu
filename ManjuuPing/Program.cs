using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Autofac;
using ManjuuDomain.HealthCheck;
using ManjuuDomain.IDomain;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace ManjuuPing
{
    class Program
    {
        private static ILogger _logger =LogManager.GetCurrentClassLogger();

        static Program()
        {
            InjectConfiguration.DeployAutoFac();
            InjectConfiguration.DeployNLog();
        }
        static void Main(string[] args)
        {

            // ICheckConfigRepository CheckConfigRepository = InjectConfiguration.Container.Resolve<ICheckConfigRepository>();
            // System.Console.WriteLine(CheckConfigRepository);


            //Task pingTask = PingCoreCode();

            //new CheckTarget("www.baidu.com", "80", "百度").TryPingAsync();
            // new CheckTarget("www.bilibili.com", "80", "哔哩哔哩").TryPingAsync();
            // new CheckTarget("www.jianshu.com", "80", "简书").TryPingAsync();
            // new CheckTarget("www.google.com", "80", "谷歌").TryPingAsync();

            // var taskController = container.Resolve<TaskController>(); //AutoFac
            InjectConfiguration.ServiceProvider.GetRequiredService<TestNLog>(); //MsILogger
            _logger.Debug("console NLog success!!!!"); //NLog

            try
            {
                
                Console.WriteLine("开始工作了");
                Console.WriteLine("主线程正在阻塞，防止程序直接退出");
                Console.WriteLine("强制退出按任意键");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            finally
            {
                Console.WriteLine("程序正在退出");
                InjectConfiguration.ServiceProvider.Dispose();
                Console.WriteLine("程序可以退出了");

            }

        }



        public static async Task PingCoreCode()
        {
            try
            {
                using (Process process = new Process())
                {
                    ProcessStartInfo startInfo = process.StartInfo;
                    startInfo.FileName = "ping";
                    startInfo.Arguments = "www.baidu.com -c 4";
                    startInfo.RedirectStandardOutput = true;
                    //startInfo.StandardOutputEncoding = Encoding.UTF8;
                    process.Start();
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = await reader.ReadToEndAsync();
                        Console.WriteLine(result);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}
