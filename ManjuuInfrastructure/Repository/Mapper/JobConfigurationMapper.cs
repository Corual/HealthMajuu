using ManjuuInfrastructure.Repository.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ManjuuInfrastructure.Repository.Mapper
{
    public class JobConfigurationMapper : BaseMapper<JobConfiguration>
    {
        public override void Configure(EntityTypeBuilder<JobConfiguration> builder)
        {
            this.ExecuteBaseMap(builder,"T_JobConfigurations");

            builder.Property(p=>p.StartToWrokTime);
            builder.Property(p=>p.StopToWrokTime);
            //没有设置,表示不等待间隔,下一轮直接开始
            builder.Property(p=>p.WorkSpan);

            //默认1000ms,数据库预设
            builder.Property(p=>p.PresetTimeout).IsRequired();
            //默认4次,数据库预设
            builder.Property(p=>p.PingSendCount).IsRequired();

            
        }
    }
}