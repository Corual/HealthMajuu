using ManjuuApplications;
using ManjuuCommon.ILog;
using ManjuuCommon.ILog.NLog;
using ManjuuDomain.Dto;
using Microsoft.Extensions.Hosting;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ManjuuPing
{
    public class PingHostedService : IHostedService
    {

        private IProgramLog<ILogger> _programLog;
        private IExceptionLog<ILogger> _errorLog;
        private IToolConfigApplication _toolConfigApplication;

        public PingHostedService(IProgramLog<ILogger> programLog, IExceptionLog<ILogger> errorLog, IToolConfigApplication toolConfigApplication)
        {
            _programLog = programLog;
            _errorLog = errorLog;
            _toolConfigApplication = toolConfigApplication;
        }

        #region 启动
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            NLogMgr.DebugLog(_programLog, "正在加载工具配置");

            var toolSetting = await LoadToolConfigAsync();

            if (null == toolSetting)
            {
                NLogMgr.DebugLog(_programLog, "尚未有可用配置，进入等待状态");
                //todo:使用定时框架，定时获取配置
                return;
            }


            //todo:根据配置，生成定时任务

            return ;
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

    }
}
