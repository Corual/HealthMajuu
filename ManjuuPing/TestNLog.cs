using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManjuuPing
{
    public class TestNLog
    {
        private ILogger<TestNLog> _logger;
        public TestNLog(ILogger<TestNLog> logger)
        {
            _logger = logger;
            _logger.LogError("testError");
        }
    }
}
