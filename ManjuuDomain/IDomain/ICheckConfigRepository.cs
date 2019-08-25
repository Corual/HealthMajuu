using System.Collections.Generic;
using ManjuuDomain.HealthCheck;

namespace ManjuuDomain.IDomain
{
    /// <summary>
    /// 检测配置仓储接口
    /// </summary>
    public interface ICheckConfigRepository
    {
        /// <summary>
        /// 添加配置数据
        /// </summary>
        /// <param name="config">配置聚合根</param>
        /// <returns></returns>
        bool AddConfigData(CheckConfig config);

        /// <summary>
        /// 获取所有可用配置
        /// </summary>
        /// <returns></returns>
        List<CheckConfig> GetValidConfigs();

        /// <summary>
        /// 更新配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        bool UpdateConfigData(CheckConfig config);

    }
}