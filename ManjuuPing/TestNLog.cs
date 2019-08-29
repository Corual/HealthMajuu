using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManjuuPing
{
    public class TestNLog
    {
        private ILogger _logger;
        public TestNLog(ILogger logger)
        {
            _logger = logger;
            
        }
    }
}
