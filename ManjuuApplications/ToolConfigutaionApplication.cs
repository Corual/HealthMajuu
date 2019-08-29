using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ToolConfigutaionApplication: IToolConfigApplication
    {
        private ICheckConfigRepository Repository;

        public ToolConfigutaionApplication(ICheckConfigRepository repository)
        {
            Repository = repository;
        }

        public async Task<ToolConfigDto> GetToolValidConfigAsync()
        {
            List<ToolConfigDto> list = await Repository.GetValidConfigsAsync();
            ToolConfigDto result = null;
            if (list != null && list.Any())
            {
                result = list[0];
            }

            return result;
        }

        /// <summary>
        /// 添加配置
        /// </summary>
        /// <param name="newConfiguration"></param>
        /// <returns></returns>
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

            var success = await Repository.AddConfigDataAsync(checkConfig);

            if (!success)
            {
                return new JsonDataMsg<string>(null, success, "增添配置异常");
            }

            //todo:异步IPC通知工具客户端配置已变化

            return new JsonDataMsg<string>(null, success, "已经为您加入该配置");
        }

        /// <summary>
        /// 修改配置
        /// </summary>
        /// <param name="newConfiguration"></param>
        /// <returns></returns>
        public async Task<object> UserAlterConfigurationToToolAsync(ToolConfigDto newConfiguration)
        {
            //使用mapper装换成聚合根
            MapperConfiguration cfg = EntityAutoMapper.Instance.AutoMapperConfig(nameof(JobConfiguration));
            CheckConfig checkConfig = EntityAutoMapper.Instance.GetMapperResult<CheckConfig>(cfg, newConfiguration);

            var success = await Repository.UpdateConfigDataAsync(checkConfig);

            if (!success)
            {
                return new JsonDataMsg<string>(null, success, "更改配置发生异常");
            }

            //todo:异步IPC通知工具客户端配置已变化

            return new JsonDataMsg<string>(null, success, "您已成功更改配置");
        }
    }
}