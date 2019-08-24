using ManjuuInfrastructure.Repository.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ManjuuInfrastructure.Repository.Mapper
{
    /// <summary>
    /// 定义映射配置的接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMapper<T>
    where T:BaseEntity
    {
         void ExecuteBaseMap(EntityTypeBuilder<T> builder, string tableName);
    }
}