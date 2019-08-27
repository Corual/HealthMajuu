using AutoMapper;
using ManjuuDomain.Dto;
using ManjuuDomain.IDomain;
using ManjuuInfrastructure.Repository.Context;
using ManjuuInfrastructure.Repository.Entity;
using ManjuuInfrastructure.Repository.Enum;
using ManjuuInfrastructure.Repository.Mapper.Auto;
using Microsoft.EntityFrameworkCore;
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
                        //todo:日志记录异常情况
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
                System.Console.WriteLine(ex.Message);
                //todo:日志记录异常
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

                List<MachineInfo>  machineInfoList = EntityAutoMapper.Instance.GetMapperResult<List<MachineInfo>>(_mapperCfg, equipmentDtos);
                if (null == machineInfoList || !machineInfoList.Any())
                {
                    //todo:日志记录异常情况
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
                            Console.WriteLine("重新替换检设备数据，正在删除旧数据");

                            //把以前的数据清空
                           var deletedCoount = await  context.Database.ExecuteSqlCommandAsync(new RawSqlString($"delete from {nameof(MachineInfo)}s"));

                            Console.WriteLine($"删除完成，共删除{deletedCoount.ToString()}条数据");

                            Console.WriteLine("进行批量导入数据");

                           await  context.MachineInfos.AddRangeAsync(machineInfoList);
                           int insertCount = await  context.MachineInfos.CountAsync();

                            if (insertCount > 0)
                            {
                                transaction.Commit();
                                await context.SaveChangesAsync();
                                Console.WriteLine($"成功导入{insertCount}条数据");
                            }
                            else
                            {
                                transaction.Rollback();
                                Console.WriteLine($"数据导入失败");
                            }


                            return insertCount > 0;
                        }
                        catch (System.Exception ex)
                        {
                            System.Console.WriteLine(ex.Message);
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
                System.Console.WriteLine(ex.Message);
                //todo:日志记录异常
                return false;
            }
        }
    }
}
