using AutoMapper;
using ManjuuCommon.ILog;
using ManjuuCommon.ILog.NLog;
using ManjuuDomain.Dto;
using ManjuuDomain.IDomain;
using ManjuuInfrastructure.Repository.Context;
using ManjuuInfrastructure.Repository.Entity;
using ManjuuInfrastructure.Repository.Enum;
using ManjuuInfrastructure.Repository.Mapper.Auto;
using Microsoft.EntityFrameworkCore;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManjuuInfrastructure.Repository
{
    public class CheckTargetRepository : ICheckTargetRepository
    {

        private MapperConfiguration _mapperCfg = EntityAutoMapper.Instance.AutoMapperConfig(nameof(MachineInfo));

        private IExceptionLog<ILogger> _errorLog;
        private IProgramLog<ILogger> _programLog;

        public CheckTargetRepository(IExceptionLog<ILogger> errorLog, IProgramLog<ILogger> programLog)
        {
            _errorLog = errorLog;
            _programLog = programLog;
        }

        public async Task<DataBoxDto<EquipmentDto>> QuantitativeTargetsAsync(int current, int capacity)
        {
            try
            {
                using (HealthManjuuCoreContext context = new HealthManjuuCoreContext())
                {

                    var query = context.MachineInfos.Where(p => p.State != DataState.Disable).AsNoTracking();
                    if (!await query.AnyAsync())
                    {
                        return new DataBoxDto<EquipmentDto>();
                    }

                    int total = await query.CountAsync();

                    query.OrderBy(p => p.Id);
                    if (capacity > 0)
                    {
                        query = query.Skip((current - 1) * capacity).Take(capacity);
                    }


                    List<MachineInfo> machineInfos = await query.ToListAsync();

                    var eqs = EntityAutoMapper.Instance.GetMapperResult<List<EquipmentDto>>(_mapperCfg, machineInfos);

                    if (null == eqs || !eqs.Any())
                    {
                        NLogMgr.ErrorExLog(_errorLog, " List<MachineInfo> 转换 List<EquipmentDto>失败", null);
                        return new DataBoxDto<EquipmentDto>();
                    }

                    var box = new DataBoxDto<EquipmentDto>();
                    box.Data = eqs;
                    box.Total = total;

                    return box;

                }
            }
            catch (System.Exception ex)
            {
                NLogMgr.ErrorExLog(_errorLog, " 批量获取检测目标异常", ex);
                return new DataBoxDto<EquipmentDto>();
            }
        }

        public async Task<bool> ReplaceTargetsAsync(List<EquipmentDto> equipmentDtos)
        {
            try
            {
                if (null == equipmentDtos || !equipmentDtos.Any())
                {
                    return false;
                }

                List<MachineInfo> machineInfoList = EntityAutoMapper.Instance.GetMapperResult<List<MachineInfo>>(_mapperCfg, equipmentDtos);
                if (null == machineInfoList || !machineInfoList.Any())
                {
                    NLogMgr.ErrorExLog(_errorLog, " List<EquipmentDto>转换List<MachineInfo>异常", null);
                    return false;
                }

                using (HealthManjuuCoreContext context = new HealthManjuuCoreContext())
                {
                    //时间问题，不考虑并发修改问题

                    using (var transaction = await context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            //走这步之前，记得IPC通知客户端通知检测
                            //Console.WriteLine("重新替换检设备数据，正在删除旧数据");
                            NLogMgr.DebugLog(_programLog, "重新替换检设备数据，正在删除旧数据");

                            //把以前的数据清空
                            var deletedCoount = await context.Database.ExecuteSqlCommandAsync(new RawSqlString($"delete from {nameof(MachineInfo)}s"));

                            //Console.WriteLine($"删除完成，共删除{deletedCoount.ToString()}条数据");
                            NLogMgr.DebugLog(_programLog, $"删除完成，共删除{deletedCoount.ToString()}条数据");

                            //Console.WriteLine("进行批量导入数据");
                            NLogMgr.DebugLog(_programLog, "进行批量导入数据");

                            await context.MachineInfos.AddRangeAsync(machineInfoList);
                            transaction.Commit();
                            await context.SaveChangesAsync();

                            //Console.WriteLine($"成功导入{machineInfoList.Count}条数据");
                            NLogMgr.DebugLog(_programLog, $"成功导入{machineInfoList.Count}条数据");


                            return true;
                        }
                        catch (System.Exception ex)
                        {
                            //System.Console.WriteLine(ex.Message);

                            NLogMgr.ErrorExLog(_errorLog, "批量导入检测目标数据异常", ex);

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
                //System.Console.WriteLine(ex.Message);
                NLogMgr.ErrorExLog(_errorLog, "方法运行异常", ex);
                return false;
            }
        }
    }
}
