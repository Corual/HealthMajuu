using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Autofac;
using ManjuuCommon.Log;
using ManjuuCommon.Log.NLog;
using ManjuuDomain.HealthCheck;
using ManjuuDomain.IDomain;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace ManjuuPing
{
    class Program
    {
        private static ILogger _logger = null;

        static Program()
        {
            InjectConfiguration.DeployAutoFac();
            _logger = InjectConfiguration.Container.Resolve<IDefaultLog<ILogger>>().GetLogger();

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



            LogEventInfo theEvent = NLogMgr.GetEventInfo(LogLevel.Debug, "", NLogMgr.LoggerName.Check);
            NLogMgr.SetEventProperties(theEvent, 
                new SetEventPropertieParam() {Property= NLogMgr.EventProperties.CheckTarget, Value= "bilibili" },
                new SetEventPropertieParam() { Property = NLogMgr.EventProperties.CheckMsg, Value = "Ping 丢包100%" },
                new SetEventPropertieParam() { Property = NLogMgr.EventProperties.CheckResult, Value = "超时\r\n超时\r\n超时\r\n超时fdsfkdsfsldkl" });
            //theEvent.LoggerName = "CHECK_LOGGER";
            //theEvent.Properties["CheckTarget"] = "bilibili";
            //theEvent.Properties["CheckMsg"] = "Ping 丢包100%";
            //theEvent.Properties["CheckResult"] = "超时\r\n超时\r\n超时\r\n超时fdsfkdsfsldkl";
            var checkLogger = InjectConfiguration.Container.Resolve<ICheckLog<ILogger>>().GetLogger();

            checkLogger.Log(theEvent);
            //theEvent.Properties["CheckLogType"] = "short";
            //_logger.Factory.GetLogger("CHECK_LOGGER").Log(theEvent);
            //_logger.Log(theEvent);




            try
            {
                Console.WriteLine("开始工作了");
                Console.WriteLine("主线程正在阻塞，防止程序直接退出");
                Console.WriteLine("强制退出按任意键");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                //_logger.Error(ex.Message);

                LogEventInfo theEventError = new LogEventInfo(LogLevel.Error, "", "测试自定义目标");
                theEventError.Exception = ex;
                //theEventError.Properties["CheckTarget"] = "bilibili";
                //theEvent.Properties["CheckLogType"] = "short";
                _logger.Log(theEventError);
            }
            finally
            {
                Console.WriteLine("程序正在退出");
                //InjectConfiguration.ServiceProvider.Dispose();
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
