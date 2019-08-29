using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ManjuuCommon.ILog.NLog;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace ManjuuManagement
{
    public class Program
    {
        
        public static void Main(string[] args)
        {

            #region 不需要了
            //var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            //try
            //{
            //    logger.Debug("init main");
            //    CreateWebHostBuilder(args).Build().Run();
            //}
            //catch (Exception ex)
            //{
            //    //NLog: catch setup errors
            //    logger.Error(ex, "Stopped program because of exception");
            //    throw;
            //}
            //finally
            //{
            //    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
            //    NLog.LogManager.Shutdown();
            //} 
            #endregion

            CreateWebHostBuilder(args).Build().Run();

        }

        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>();
        //     //   .ConfigureLogging(logging =>
        //     //   {
        //     //       logging.ClearProviders();
        //     //       logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
        //     //   })
        //     //.UseNLog();  // NLog: setup NLog for Dependency injection;

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            NLogMgr.SetVariable( NLogMgr.ConfigurationVariables.Terrace, "测试工具管理后台");
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        }
    }
}
