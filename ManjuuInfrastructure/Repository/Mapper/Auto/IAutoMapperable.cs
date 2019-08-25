using AutoMapper;

namespace ManjuuInfrastructure.Repository.Mapper.Auto
{
    public interface IAutoMapperable
    {
         MapperConfiguration  MapperInitialize();
    }
}