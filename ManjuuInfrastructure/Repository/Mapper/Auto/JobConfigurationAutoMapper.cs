using AutoMapper;
using ManjuuDomain.HealthCheck;
using ManjuuInfrastructure.Repository.Entity;
using System.Collections.Generic;

namespace ManjuuInfrastructure.Repository.Mapper.Auto {
    public class JobConfigurationAutoMapper : IAutoMapperable {
        public MapperConfiguration MapperInitialize () {
            return new MapperConfiguration (cfg => {
                cfg.CreateMap<JobConfiguration, CheckConfig> ();
                cfg.CreateMap<List<JobConfiguration>, List<CheckConfig>>();
            });

        }
    }
}