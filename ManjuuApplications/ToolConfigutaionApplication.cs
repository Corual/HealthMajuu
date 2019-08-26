using System.Threading.Tasks;
using AutoMapper;
using ManjuuCommon.DataPackages;
using ManjuuDomain.Dto;
using ManjuuDomain.HealthCheck;
using ManjuuDomain.IDomain;
using ManjuuInfrastructure.Repository.Entity;
using ManjuuInfrastructure.Repository.Mapper.Auto;

namespace ManjuuApplications
{
    public class ToolConfigutaionApplication
    {
        private ICheckConfigRepository Repository;
        public ToolConfigutaionApplication(ICheckConfigRepository repository)
        {
            Repository = repository;
        }
        public async Task<JsonDataMsg<string>> UserAddConfigurationToToolAsync(ToolConfigDto newConfiguration)
        {
            //参数为空检查
            if (null == newConfiguration)
            {
                return new JsonDataMsg<string>(null, false, "后台接收数据失败");
            }

            //使用mapper装换成聚合根
            MapperConfiguration cfg = EntityAutoMapper.Instance.AutoMapperConfig(nameof(JobConfiguration));
            CheckConfig checkConfig = EntityAutoMapper.Instance.GetMapperResult<CheckConfig>(cfg, newConfiguration);

            var success = true;//await Repository.AddConfigDataAsync(checkConfig);

            if (!success)
            {
                return new JsonDataMsg<string>(null, success, "增添配置异常");
            }

            //todo:异步IPC通知工具客户端配置已变化

            return new JsonDataMsg<string>(null, success, "以为您加入该配置");
        }
    }
}