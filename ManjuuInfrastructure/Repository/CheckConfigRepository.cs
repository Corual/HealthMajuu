using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JKang.IpcServiceFramework;
using ManjuuCommon.ILog;
using ManjuuCommon.ILog.NLog;
using ManjuuDomain.Dto;
using ManjuuDomain.HealthCheck;
using ManjuuDomain.IDomain;
using ManjuuInfrastructure.IpcService.ServiceContract;
using ManjuuInfrastructure.Repository.Context;
using ManjuuInfrastructure.Repository.Entity;
using ManjuuInfrastructure.Repository.Enum;
using ManjuuInfrastructure.Repository.Mapper.Auto;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace ManjuuInfrastructure.Repository
{
    /// <summary>
    /// 检测配置仓储
    /// </summary>
    public class CheckConfigRepository : ICheckConfigRepository
    {
        private MapperConfiguration _mapperCfg = EntityAutoMapper.Instance.AutoMapperConfig(nameof(JobConfiguration));
        private IExceptionLog<ILogger> _errorLog;


        public CheckConfigRepository(IExceptionLog<ILogger> errorLog)
        {
            _errorLog = errorLog;
        }

        public async Task<bool> AddConfigDataAsync(CheckConfig config)
        {

            try
            {
                JobConfiguration jobConfiguration = EntityAutoMapper.Instance.GetMapperResult<JobConfiguration>(_mapperCfg, config);
                if (null == jobConfiguration)
                {
                    return false;
                }

                using (HealthManjuuCoreContext context = new HealthManjuuCoreContext())
                {
                    return await AddConfigDataAsync(context, jobConfiguration);
                }
            }
            catch (System.Exception ex)
            {
                NLogMgr.ErrorExLog(_errorLog,"添加检测工具配置异常",ex);
                return false;
            }
        }

        public async Task<bool> AddConfigDataAsync(HealthManjuuCoreContext context, JobConfiguration entity)
        {
            await context.JobConfigurations.AddAsync(entity);
            int count = await context.SaveChangesAsync();
            return count > 0;
        }

        public async Task<List<ToolConfigDto>> GetValidConfigsAsync()
        {
            try
            {
                using (HealthManjuuCoreContext context = new HealthManjuuCoreContext())
                {

                    var query = context.JobConfigurations.Where(p => p.State != DataState.Disable).AsNoTracking();
                    if (!await query.AnyAsync())
                    {
                        return null;
                    }

                    List<JobConfiguration> jobList = await query.ToListAsync();

                    return EntityAutoMapper.Instance.GetMapperResult<List<ToolConfigDto>>(_mapperCfg, jobList);


                }
            }
            catch (System.Exception ex)
            {
                NLogMgr.ErrorExLog(_errorLog, "获取检测工具配置异常", ex);
                return null;
            }
        }


        public async Task<bool> UpdateConfigDataAsync(CheckConfig config)
        {
            try
            {
                if (null == config)
                {
                    return false;
                }

                JobConfiguration jobConfiguration = EntityAutoMapper.Instance.GetMapperResult<JobConfiguration>(_mapperCfg, config);
                if (null == jobConfiguration)
                {
                    return false;
                }

                using (HealthManjuuCoreContext context = new HealthManjuuCoreContext())
                {
                    //时间问题，不考虑并发修改问题

                    using (var transaction = await context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var query = context.JobConfigurations.Where(p => p.Id == config.Id && p.State == DataState.Enable);
                            if (await query.AnyAsync())
                            {
                                JobConfiguration jobCfg = await query.FirstAsync();
                                jobCfg.State = DataState.Disable;
                            }

                            jobConfiguration.Id = 0; //数据库会自动生成，把id重置为默认值
                            bool result = await AddConfigDataAsync(context, jobConfiguration);

                            if (result)
                            {
                                transaction.Commit();
                                await context.SaveChangesAsync();

                            }
                            else
                            {
                                transaction.Rollback();
                            }

                            return result;
                        }
                        catch (System.Exception ex)
                        {
                            NLogMgr.ErrorExLog(_errorLog, "更新检测工具配置异常", ex);
                            if (null != transaction)
                            {
                                transaction.Rollback();
                            }
                            return false;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                NLogMgr.ErrorExLog(_errorLog, "执行更新工具配置方法异常", ex);
                return false;
            }
        }
    }
}