
using System.ComponentModel.DataAnnotations.Schema;
using ManjuuInfrastructure.Repository.Entity;
using ManjuuInfrastructure.Repository.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ManjuuInfrastructure.Repository.Mapper
{
    public abstract class BaseMapper<T> : IEntityTypeConfiguration<T>, IMapper<T>
    where T : BaseEntity
    {
        public abstract void Configure(EntityTypeBuilder<T> builder);

        public virtual void ExecuteBaseMap(EntityTypeBuilder<T> builder, string tableName)
        {
            builder.ToTable(tableName);

            #region 主键设置
            // builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd(); //主键ID自动增长生成
            #endregion

            #region 数据状态设置(需要去数据可配置默认值“Enable”)
            //定义枚举到数据的转换器
            var stateConverter = new ValueConverter<DateState,int>(p=>(int)p,p=>(DateState)p);
            //设置插入的时候使用数据库的默认值，也就是说生成数据局后还要去设置默认值
            builder.Property(p => p.State).HasDefaultValue(DateState.Enable).IsRequired(); 
            #endregion

             #region 数据创建时间配置 (需要去数据可配置默认值“getdate()”)
            builder.Property(p=>p.CreateTime).HasDefaultValueSql("getdate()").IsRequired();
            #endregion

            #region 数据最后更新时间配置
            builder.Property(p=>p.LastUpdateTime).ValueGeneratedOnAddOrUpdate().IsRequired();
            #endregion


        }
    }
}