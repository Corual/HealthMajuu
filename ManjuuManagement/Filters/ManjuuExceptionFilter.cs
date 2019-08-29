using ManjuuCommon.ILog;
using ManjuuCommon.ILog.NLog;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManjuuManagement.Filters
{
    public class ManjuuExceptionFilter : IExceptionFilter
    {
        private readonly IExceptionLog<ILogger>  _logger;

        public ManjuuExceptionFilter(IExceptionLog<ILogger> loggger)
        {
            _logger = loggger;
        }
        public void OnException(ExceptionContext context)
        {
            if (context == null) { return; }
            //_logger.LogError(context.Exception.Message, context.Exception);

            NLogMgr.ErrorExLog(_logger, context.Exception.Message, context.Exception);
            //context.ExceptionHandled = true;

        }
    }
}
