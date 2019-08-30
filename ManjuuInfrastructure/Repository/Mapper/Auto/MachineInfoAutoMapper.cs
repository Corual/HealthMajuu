using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ManjuuDomain.Dto;
using ManjuuDomain.HealthCheck;
using ManjuuInfrastructure.Repository.Entity;

namespace ManjuuInfrastructure.Repository.Mapper.Auto
{
    public class MachineInfoAutoMapper : IAutoMapperable
    {
        public MapperConfiguration MapperInitialize()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MachineInfo, EquipmentDto>();
                cfg.CreateMap<EquipmentDto, MachineInfo>();
                cfg.CreateMap<EquipmentDto, CheckTarget>().ConvertUsing<EquipmentDto2CheckTargetConverter>();
            });


        }

        
    }

    public class EquipmentDto2CheckTargetConverter : ITypeConverter<EquipmentDto, CheckTarget>
    {
        public CheckTarget Convert(EquipmentDto source, CheckTarget destination, ResolutionContext context)
        {
            if (null == source) { return null; }
            return new CheckTarget(source.Id, source.IpAddressV4, source.Port, source.Remarks);
        }
    }
}
