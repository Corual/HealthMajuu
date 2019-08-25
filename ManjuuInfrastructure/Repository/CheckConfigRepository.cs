using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ManjuuDomain.HealthCheck;
using ManjuuDomain.IDomain;
using ManjuuInfrastructure.Repository.Context;
using ManjuuInfrastructure.Repository.Entity;
using ManjuuInfrastructure.Repository.Mapper.Auto;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ManjuuInfrastructure.Repository.Enum;
using AutoMapper.QueryableExtensions;

namespace ManjuuInfrastructure.Repository
{
    /// <summary>
    /// 检测配置仓储
    /// </summary>
    public class CheckConfigRepository : ICheckConfigRepository
    {
        private MapperConfiguration _mapperCfg = EntityAutoMapper.Instance.AutoMapperConfig(nameof(JobConfiguration));
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
                //todo:日志记录异常
                return false;
            }
        }

        public async Task<bool> AddConfigDataAsync(HealthManjuuCoreContext context, JobConfiguration entity)
        {
            await context.JobConfigurations.AddAsync(entity);
            int count = await context.SaveChangesAsync();
            return count > 0;
        }

        public async Task<List<CheckConfig>> GetValidConfigsAsync()
        {
            try
            {
                using (HealthManjuuCoreContext context = new HealthManjuuCoreContext())
                {
                    return await context.JobConfigurations.Where(p => p.State != DataState.Disable)
                    .AsNoTracking()
                    .ProjectTo<CheckConfig>(_mapperCfg).ToListAsync();
                }
            }
            catch (System.Exception)
            {

                //todo:日志记录异常
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
                            return await AddConfigDataAsync(context, jobConfiguration);
                        }
                        catch (System.Exception ex)
                        {
                            //todo:日志记录异常
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
                //todo:日志记录异常
                return false;
            }
        }
    }
}