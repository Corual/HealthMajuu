using ManjuuApplications;
using ManjuuCommon.ILog;
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
        public Task StartAsync(CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();


            return Task.CompletedTask;
        }
        #endregion

        #region 结束
        public Task StopAsync(CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();

            return Task.CompletedTask;
        }
        #endregion


        #region MyRegion

        #endregion

    }
}
