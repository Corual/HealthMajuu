using ManjuuDomain.HealthCheck;
using ManjuuDomain.HealthCheckService;
using ManjuuDomain.IDomain;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManjuuNUnitTest
{
    public class PingTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task PingResultTest()
        {
            IPingable target = new CheckTarget(1,"www.baidu.com", "80", "百度");
            Assert.NotNull(target);

            string result = await CheckTargetService.PingRemoteTargetAsync(target);
            Assert.NotNull(result);
            Assert.IsNotEmpty(result);

           // Console.WriteLine(result);
            
        }
    }
}
