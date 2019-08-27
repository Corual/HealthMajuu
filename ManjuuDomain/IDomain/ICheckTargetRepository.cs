using ManjuuDomain.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManjuuDomain.IDomain
{
    /// <summary>
    /// 检测目标仓储
    /// </summary>
    public interface ICheckTargetRepository:IRepository
    {
        /// <summary>
        /// 更替目标
        /// </summary>
        /// <returns></returns>
        Task<bool> ReplaceTargetsAsync(List<EquipmentDto> equipmentDtos);

        /// <summary>
        /// 定量的获取目标
        /// </summary>
        /// <returns></returns>
        Task<DataBoxDto<EquipmentDto>> QuantitativeTargetsAsync(int current, int capacity);

    }
}