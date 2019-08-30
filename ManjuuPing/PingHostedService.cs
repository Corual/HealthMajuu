using ManjuuApplications;
using ManjuuCommon.ILog;
using ManjuuCommon.ILog.NLog;
using ManjuuDomain.Dto;
using ManjuuDomain.HealthCheck;
using Microsoft.Extensions.Hosting;
using NLog;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ManjuuPing
{
    public class PingHostedService : IHostedService
    {

        private readonly IProgramLog<ILogger> _programLog;
        private readonly IExceptionLog<ILogger> _errorLog;
        private IToolConfigApplication _toolConfigApplication;

        private readonly ISchedulerFactory _schedulerFactory;
        private IScheduler _scheduler;

        public PingHostedService(IProgramLog<ILogger> programLog, IExceptionLog<ILogger> errorLog, IToolConfigApplication toolConfigApplication, ISchedulerFactory schedulerFactory)
        {
            _programLog = programLog;
            _errorLog = errorLog;
            _toolConfigApplication = toolConfigApplication;
            this._schedulerFactory = schedulerFactory;
        }

        #region 启动
        public async Task StartAsync(CancellationToken cancellationToken)
        {

            //await QuartzDemo();

            NLogMgr.DebugLog(_programLog, "正在加载工具配置");

            var toolSetting = await LoadToolConfigAsync();

            if (null == toolSetting)
            {
                NLogMgr.DebugLog(_programLog, "尚未有可用配置，进入等待状态");
                //todo:使用定时框架，定时获取配置
                return;
            }

            ToolRunParam toolRunParam = CheckConfig.GetRunParam(toolSetting);



            //todo:根据配置，生成定时任务

            return;
        }
        #endregion

        #region 结束
        public Task StopAsync(CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();

            return Task.CompletedTask;
        }
        #endregion


        #region 加载工具配置
        public async Task<ToolConfigDto> LoadToolConfigAsync()
        {
            return await _toolConfigApplication.GetToolValidConfigAsync();
        }
        #endregion

        #region QuartzDemo
        public async Task<string[]> QuartzDemo()
        {
            //1、通过调度工厂获得调度器
            _scheduler = await _schedulerFactory.GetScheduler();

            //2、开启调度器
            await _scheduler.Start();

            //3、创建一个触发器
            var trigger = TriggerBuilder.Create()
                            .WithSimpleSchedule(x => x.WithIntervalInSeconds(2).RepeatForever())//每两秒执行一次
                            .Build();

            //4、创建任务
            var jobDetail = JobBuilder.Create<MyJob>()
                            .WithIdentity("job", "group")
                            .Build();

            //5、将触发器和任务器绑定到调度器中
            await _scheduler.ScheduleJob(jobDetail, trigger);
            return await Task.FromResult(new string[] { "value1", "value2" });
        }
        #endregion

    }
}
