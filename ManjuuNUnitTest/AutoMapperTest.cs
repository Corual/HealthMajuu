using System;
using AutoMapper;
using ManjuuDomain.HealthCheck;
using ManjuuInfrastructure.Repository.Entity;
using ManjuuInfrastructure.Repository.Mapper.Auto;
using NUnit.Framework;

namespace ManjuuNUnitTest
{
    public class AutoMapperTest
    {
        [Test]
        public void JobConfigurationAutoMapper()
        {
           MapperConfiguration cfg = EntityAutoMapper.Instance.AutoMapperConfig(nameof(JobConfiguration));
            Assert.IsNotNull(cfg);
            cfg.AssertConfigurationIsValid();


            string startTime = "2019-08-25 16:00:05";
            string endTime = "2019-08-25 17:30:00";
           CheckConfig result = EntityAutoMapper.Instance.GetMapperResult<CheckConfig>(cfg, new JobConfiguration(){
                StartToWrokTime = DateTime.Parse(startTime),
                StopToWorkTime = DateTime.Parse(endTime),
                WorkSpan = 3000,
                PresetTimeout = 1000,
                PingSendCount = 4
            });

            Assert.IsNotNull(result);
            Assert.AreEqual(startTime, result.StartToWrokTime.ToString("yyyy-MM-dd HH:mm:ss"));
            Assert.AreEqual(endTime, result.StopToWorkTime.ToString("yyyy-MM-dd HH:mm:ss"));
            Assert.AreEqual(3000, result.WorkSpan);
            Assert.AreEqual(1000, result.PresetTimeout);
            Assert.AreEqual(4, result.PingSendCount);

        }
    }
}