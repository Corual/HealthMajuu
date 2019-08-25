using AutoMapper;
using ManjuuDomain.HealthCheck;
using ManjuuInfrastructure.Repository.Entity;

namespace ManjuuInfrastructure.Repository.Mapper.Auto
{
    public class JobConfigurationAutoMapper : IAutoMapperable
    {
        public MapperConfiguration  MapperInitialize()
        {
           return new MapperConfiguration(cfg=>{
                cfg.CreateMap<JobConfiguration, CheckConfig>();
           });

        }
    }
}