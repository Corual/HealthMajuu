using AutoMapper;
using JKang.IpcServiceFramework;
using ManjuuApplications;
using ManjuuCommon.ILog;
using ManjuuCommon.ILog.NLog;
using ManjuuDomain.Dto;
using ManjuuDomain.HealthCheck;
using ManjuuDomain.IDomain;
using ManjuuInfrastructure.Repository.Entity;
using ManjuuInfrastructure.Repository.Mapper.Auto;
using ManjuuPing.IpcServerImp;
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
        private ICheckTargetRepository _repository;

        private readonly ISchedulerFactory _schedulerFactory;
        private IScheduler _scheduler;
        private IIpcServiceHost _ipcServiceHost;
        private CancellationTokenSource _source;

        /// <summary>
        /// 工具运行参数
        /// </summary>
        public ToolRunParam RunParam { get; private set; }

        /// <summary>
        /// 记录当前数据检测到第几页
        /// </summary>
        public int CurrentPage { get; set; } = 1;
        public int TotalPage { get; private set; } = 0;
        public int Capacity { get; private set; } = 10;

        private MapperConfiguration _mapperCfg = EntityAutoMapper.Instance.AutoMapperConfig(nameof(MachineInfo));


        public PingHostedService(IProgramLog<ILogger> programLog, IExceptionLog<ILogger> errorLog, IToolConfigApplication toolConfigApplication, ICheckTargetRepository repository, ISchedulerFactory schedulerFactory, IIpcServiceHost ipcServiceHost)
        {
            _programLog = programLog;
            _errorLog = errorLog;
            _toolConfigApplication = toolConfigApplication;
            this._schedulerFactory = schedulerFactory;
            _ipcServiceHost = ipcServiceHost;
            _repository = repository;
        }



        #region 启动
        public async Task StartAsync(CancellationToken cancellationToken)
        {

            //await QuartzDemo();

            await IpcServerRun();

            if (!await LoadToolParamAsync())
            {
                return;
            }

            //todo:根据配置，生成定时任务
            ExecuteMissions();
            return;
        }
        #endregion

        #region 结束
        public Task StopAsync(CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();


            StopScheduler();

            if (null != _source)
            {
                _source.Cancel();
                NLogMgr.DebugLog(_programLog, "IpcServer端退出");
            }
            return Task.CompletedTask;
        }
        #endregion

        #region 加载工具配置
        public async Task<ToolConfigDto> LoadToolConfigAsync()
        {

            NLogMgr.DebugLog(_programLog, "正在加载工具配置");

            var toolSetting = await _toolConfigApplication.GetToolValidConfigAsync();

            if (null == toolSetting)
            {
                NLogMgr.DebugLog(_programLog, "尚未有可用配置，进入等待状态");
                //todo:使用定时框架，定时获取配置
                return null;
            }

            return toolSetting;

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
            var trigger = TriggerBuilder.Create().WithDailyTimeIntervalSchedule(p => p.OnEveryDay()
                .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(7, 0))
                .EndingDailyAt(TimeOfDay.HourAndMinuteOfDay(12, 30))
                .WithRepeatCount(int.MaxValue))
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

        #region IpcServer
        public Task IpcServerRun()
        {
            if (null == _ipcServiceHost) { return Task.CompletedTask; }

            NLogMgr.DebugLog(_programLog, "IpcServer端启动");

            PingServiceContract.ResponseConfigModified += ToolCofigurationModified;
            PingServiceContract.ResponseJobRestart += JobRestart;
            PingServiceContract.ResponseStopJob += StopJob;

            _source = new CancellationTokenSource();
            _ipcServiceHost.RunAsync(_source.Token);

            return Task.CompletedTask;
        }
        #endregion

        #region IpcCallBack
        private async Task ToolCofigurationModified()
        {
            NLogMgr.DebugLog(_programLog, "收到工具配置改动消息");
            await PauseScheduler();
            await StopScheduler();
            if (!await LoadToolParamAsync())
            {
                return;
            }

            await ExecuteMissions();

        }

        private async Task<bool> StopJob()
        {
            NLogMgr.DebugLog(_programLog, "收到暂停作业消息");
            await PauseScheduler();
            return true;
        }

        private async Task JobRestart()
        {
            NLogMgr.DebugLog(_programLog, "收到重新作业消息");
            await RestartScheduler();
        }
        #endregion

        #region 异步获取工具参数
        private async Task<bool> LoadToolParamAsync()
        {
            var toolSetting = await LoadToolConfigAsync();

            if (null == toolSetting) { return false; }

            RunParam = CheckConfig.GetRunParam(toolSetting);

            return null != RunParam;
        }

        #endregion

        #region 执行任务
        private async Task ExecuteMissions()
        {
            _scheduler = await _schedulerFactory.GetScheduler();
            Quartz.ITrigger trigger = null;


            if (RunParam.WorkType == ToolWorkType.AllDay)
            {
                //每分钟执行一次
                trigger = TriggerBuilder.Create().WithSimpleSchedule(p => p.WithIntervalInMinutes(1)).Build();
            }
            else if (RunParam.WorkType == ToolWorkType.TimeToTime)
            {
                string[] start = RunParam.TimeToStart.Split(':');
                string[] stop = RunParam.TimeToStop.Split(':');
                trigger = TriggerBuilder.Create().WithDailyTimeIntervalSchedule(p => p.OnEveryDay()
                .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(int.Parse(start[0]), int.Parse(start[1])))
                .EndingDailyAt(TimeOfDay.HourAndMinuteOfDay(int.Parse(stop[0]), int.Parse(stop[1])))
                .WithIntervalInSeconds(10))
                    .Build();
            }
            else
            {
                //未支持，立马返回
                return;
            }

            IJobDetail jobDetail = JobBuilder.Create<PingJob>().WithIdentity("ping", "healthCheck").UsingJobData(new JobDataMap(new Dictionary<string, PingHostedService> { { "runData", this } })).Build();

            await _scheduler.Start();
            NLogMgr.DebugLog(_programLog, "定时任务开始");
            await _scheduler.ScheduleJob(jobDetail, trigger);

            return;

        }
        #endregion

        #region 获取页数据
        public async Task<List<CheckTarget>> GetDataPage(int page)
        {
            DataBoxDto<EquipmentDto> dataBoxDto = await _repository.QuantitativeTargetsAsync(CurrentPage, Capacity);
            if (0 == dataBoxDto.Total)
            {
                NLogMgr.DebugLog(_programLog, "首次获取数据，发现没有可检测的目标");
                return null;
            }

            if (1 == page)
            {
                //有数据则根据数据，得到总分页数，便于后续遍历
                TotalPage = (int)Math.Ceiling(dataBoxDto.Total * 1.0 / Capacity);
            }


            return EntityAutoMapper.Instance.GetMapperResult<List<CheckTarget>>(_mapperCfg, dataBoxDto.Data);


        }
        #endregion

        #region StopScheduler
        private async Task StopScheduler()
        {
            if (null != _scheduler && !_scheduler.IsShutdown)
            {
                await _scheduler.Shutdown();
                NLogMgr.DebugLog(_programLog, "定时任务结束");
            }
        }
        #endregion

        #region PauseScheduler
        private async Task PauseScheduler()
        {
            if (null != _scheduler && _scheduler.IsStarted)
            {
                await _scheduler.PauseJob(new JobKey("ping"));
                NLogMgr.DebugLog(_programLog, "定时任务暂停");
            }
        }
        #endregion


        #region RestartScheduler
        private async Task RestartScheduler()
        {
            if (null != _scheduler && _scheduler.IsShutdown)
            {
                await _scheduler.ResumeJob(new JobKey("ping"));
                NLogMgr.DebugLog(_programLog, "定时任务恢复工作");
            }
        }
        #endregion



    }
}
