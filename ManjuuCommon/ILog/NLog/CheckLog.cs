using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManjuuCommon.ILog.NLog
{
    public class CheckLog : ICheckLog<ILogger>
    {
        public ILogger GetLogger()
        {
            return LogManager.LogFactory.GetLogger(NLogMgr.LoggerName.Check);
        }
    }
}
