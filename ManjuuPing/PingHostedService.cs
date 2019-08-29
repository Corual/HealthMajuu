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
        public PingHostedService(IProgramLog<ILogger> programLog, IExceptionLog<ILogger> errorLog)
        {
            _programLog = programLog;
            _errorLog = errorLog;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();

            return Task.CompletedTask;
        }
    }
}
