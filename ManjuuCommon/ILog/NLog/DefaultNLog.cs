using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManjuuCommon.ILog.NLog
{
    public class DefaultNLog : IDefaultLog<ILogger>
    {
        public ILogger GetLogger()
        {
            return LogManager.GetCurrentClassLogger();
        }
    }
}
