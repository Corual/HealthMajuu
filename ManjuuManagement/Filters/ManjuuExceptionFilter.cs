using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManjuuManagement.Filters
{
    public class ManjuuExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ManjuuExceptionFilter> _logger;

        public ManjuuExceptionFilter(ILogger<ManjuuExceptionFilter> loggger)
        {
            _logger = loggger;
        }
        public void OnException(ExceptionContext context)
        {
            if (context == null) { return; }
            _logger.LogError(context.Exception.Message, context.Exception);

            //context.ExceptionHandled = true;

        }
    }
}
