using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ManjuuDomain.Dto;
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
            });


        }
    }
}
