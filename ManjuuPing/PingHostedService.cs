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
        private ToolRunParam _runParam;

        /// <summary>
        /// 记录当前数据检测到第几页
        /// </summary>
        private int _currentPage = 1;
        private int _totalPage = 0;
        private int _capacity = 10;

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
            CreateMissions();
            return;
        }
        #endregion

        #region 结束
        public Task StopAsync(CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();

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
        private void ToolCofigurationModified()
        {
            NLogMgr.DebugLog(_programLog, "收到工具配置改动消息");

        }

        private bool StopJob()
        {
            NLogMgr.DebugLog(_programLog, "收到停止作业消息");
            return false;
        }

        private void JobRestart()
        {
            NLogMgr.DebugLog(_programLog, "收到重新作业消息");
        }
        #endregion

        #region 异步获取工具参数
        private async Task<bool> LoadToolParamAsync()
        {
            var toolSetting = await LoadToolConfigAsync();

            if (null == toolSetting) { return false; }

            _runParam = CheckConfig.GetRunParam(toolSetting);

            return null != _runParam;
        }

        #endregion

        #region 创建任务
        private async Task CreateMissions()
        {
            var list = await GetDataPage(_currentPage);

            if (null == list || 0 == list.Count)
            {
                return;
            }

            foreach (var item in list)
            {
                item.TryPingAsync();
            }


        }
        #endregion

        #region 获取页数据
        private async Task<List<CheckTarget>> GetDataPage(int page)
        {
            DataBoxDto<EquipmentDto> dataBoxDto = await _repository.QuantitativeTargetsAsync(_currentPage, _capacity);
            if (0 == dataBoxDto.Total)
            {
                NLogMgr.DebugLog(_programLog, "首次获取数据，发现没有可检测的目标");
                return null;
            }

            if (1 == page)
            {
                //有数据则根据数据，得到总分页数，便于后续遍历
                _totalPage = (int)Math.Ceiling(dataBoxDto.Total * 1.0 / _capacity);
            }


            return EntityAutoMapper.Instance.GetMapperResult<List<CheckTarget>>(_mapperCfg, dataBoxDto.Data);


        }
        #endregion



    }
}
