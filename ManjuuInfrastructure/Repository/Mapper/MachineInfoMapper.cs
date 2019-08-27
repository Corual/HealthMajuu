using ManjuuInfrastructure.Repository.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManjuuInfrastructure.Repository.Mapper
{
    public class MachineInfoMapper  : BaseMapper<MachineInfo>
    {
        public override void Configure(EntityTypeBuilder<MachineInfo> builder)
        {
            this.ExecuteBaseMap(builder, "T_MachineInfos");

            builder.Property(p => p.IpAddressV4).IsRequired(true);
            builder.HasIndex(p => p.IpAddressV4).IsUnique(true);
            builder.Property(p => p.Port).IsRequired().HasMaxLength(5);
          
            builder.Property(p => p.Remarks).IsRequired(false).IsUnicode();


        }
    }
}
