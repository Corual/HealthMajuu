using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JKang.IpcServiceFramework;
using ManjuuCommon.DataPackages;
using ManjuuCommon.ILog;
using ManjuuCommon.ILog.NLog;
using ManjuuDomain.Dto;
using ManjuuDomain.HealthCheck;
using ManjuuDomain.IDomain;
using ManjuuInfrastructure.IpcService.ServiceContract;
using ManjuuInfrastructure.Repository.Entity;
using ManjuuInfrastructure.Repository.Mapper.Auto;
using NLog;

namespace ManjuuApplications
{
    public class ToolConfigutaionApplication : IToolConfigApplication
    {
        private ICheckConfigRepository Repository;
        private IExceptionLog<ILogger> _errorLog;
        private IProgramLog<ILogger> _programLog;

        private IpcServiceClient<ICheckConfigServiceContract> _client;


        public ToolConfigutaionApplication(ICheckConfigRepository repository, IProgramLog<ILogger> programLog, IExceptionLog<ILogger> errorLog, IpcServiceClient<ICheckConfigServiceContract> client)
        {
            Repository = repository;
            _programLog = programLog;
            _client = client;
            _errorLog = errorLog;
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
                NLogMgr.DebugLog(_programLog, "后台接收配置数据失败");
                return new JsonDataMsg<string>(null, false, "后台接收数据失败");
            }



            //使用mapper装换成聚合根
            MapperConfiguration cfg = EntityAutoMapper.Instance.AutoMapperConfig(nameof(JobConfiguration));
            CheckConfig checkConfig = EntityAutoMapper.Instance.GetMapperResult<CheckConfig>(cfg, newConfiguration);

            var success = await Repository.AddConfigDataAsync(checkConfig);

            if (!success)
            {
                NLogMgr.DebugLog(_programLog, "增添配置异常");
                return new JsonDataMsg<string>(null, success, "增添配置异常");
            }

            //异步IPC通知工具客户端配置已变化
            _client.InvokeAsync(p => p.ConfigWasModify());

            NLogMgr.DebugLog(_programLog, "已经为您加入该配置");
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
                NLogMgr.DebugLog(_programLog, "更改配置发生异常");
                return new JsonDataMsg<string>(null, success, "更改配置发生异常");
            }

            //异步IPC通知工具客户端配置已变化
            _client.InvokeAsync(p => p.ConfigWasModify());

            NLogMgr.DebugLog(_programLog, "您已成功更改配置");
            return new JsonDataMsg<string>(null, success, "您已成功更改配置");
        }
    }
}