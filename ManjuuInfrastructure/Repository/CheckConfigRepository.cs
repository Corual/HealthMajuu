using System.Collections.Generic;
using ManjuuDomain.HealthCheck;
using ManjuuDomain.IDomain;

namespace ManjuuInfrastructure.Repository
{
    /// <summary>
    /// 检测配置仓储
    /// </summary>
    public class CheckConfigRepository : ICheckConfigRepository
    {
        public bool AddConfigData(CheckConfig config)
        {
            throw new System.NotImplementedException();
        }

        public List<CheckConfig> GetValidConfigs()
        {
            throw new System.NotImplementedException();
        }

        public bool UpdateConfigData(CheckConfig config)
        {
            throw new System.NotImplementedException();
        }
    }
}