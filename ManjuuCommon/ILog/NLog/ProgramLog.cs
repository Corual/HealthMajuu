using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManjuuCommon.ILog.NLog
{
   public class ProgramLog : IProgramLog<ILogger>
    {
        public ILogger GetLogger()
        {
            return LogManager.LogFactory.GetLogger(NLogMgr.LoggerName.Program);
        }
    }
}
