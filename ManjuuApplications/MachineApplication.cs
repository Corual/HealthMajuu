using ManjuuCommon.DataPackages;
using ManjuuCommon.ILog;
using ManjuuCommon.ILog.NLog;
using ManjuuCommon.Tools;
using ManjuuDomain.Dto;
using ManjuuDomain.IDomain;
using Microsoft.AspNetCore.Http;
using NLog;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManjuuApplications
{
    public class MachineApplication : IMachineApplication
    {
        private ICheckTargetRepository Repository;

        private IProgramLog<ILogger> _programLog;
        private IExceptionLog<ILogger> _errorLog;

        public MachineApplication(ICheckTargetRepository repository, IProgramLog<ILogger> programLog, IExceptionLog<ILogger> errorLog)
        {
            Repository = repository;
            _programLog = programLog;
            _errorLog = errorLog;
        }

        public async Task<PageMsg<EquipmentDto>> PaggingMachinesAsync(int current, int capacity = 20)
        {
            if (current < 1)
            {
                current = 1;
            }

            DataBoxDto<EquipmentDto> dataBoxDto = await Repository.QuantitativeTargetsAsync(current, capacity);
            if (0 == dataBoxDto.Total)
            {
                return new PageMsg<EquipmentDto>() { Msg = "获取数据异常或者没数据", BusinessResult = false };
            }

            PageMsg<EquipmentDto> pageMsg = new PageMsg<EquipmentDto>(dataBoxDto.Data, current, dataBoxDto.Total, capacity);
            pageMsg.Msg = "获取成功";
            pageMsg.BusinessResult = true;
            return pageMsg;
        }

        public async Task<JsonDataMsg<string>> ImportingMachinesAsync(IFormCollection formCollection)
        {

            if (null == formCollection || null == formCollection.Files || !formCollection.Files.Any())
            {
                NLogMgr.ErrorExLog(_errorLog, "服务器接收不到文件Excel文件", null);
                return new JsonDataMsg<string>(null, false, "服务器接收不到文件");
            }

            IFormFile file = formCollection.Files[0];

            string extension = Path.GetExtension(file.FileName);

            //var fileExtensions = new string[] { ".xlsx", ".xls", ".csv" };
            //if (!fileExtensions.Contains(extension))
            //{
            //    return new JsonDataMsg<string>(null, false, "您上传的文件不是Excel文件");
            //}

            if (".xlsx" != extension)
            {
                NLogMgr.DebugLog(_programLog, "您上传的文件不是*.xlsx后缀的文件");
                return new JsonDataMsg<string>(null, false, "您上传的文件不是*.xlsx后缀的文件");
            }

            //todo:导入前通知工具退出

            List<EquipmentDto> list = null;
            using (MemoryStream stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);

                list = ExcelOptr.ResolveExcelFile<EquipmentDto>(stream,
                  new ExcelMapper[] { new ExcelMapper("ip",nameof(EquipmentDto.IpAddressV4)),
                     new ExcelMapper("备注",nameof(EquipmentDto.Remarks))});
            }


            bool success = await Repository.ReplaceTargetsAsync(list);

            JsonDataMsg<string> result = null;
            if (success)
            {
                NLogMgr.DebugLog(_programLog, "设备导入完毕");
                result = new JsonDataMsg<string>(null, success, "设备导入完毕");
            }
            else
            {
                NLogMgr.DebugLog(_programLog, "导入设备操作过程发生异常");
                result = new JsonDataMsg<string>(null, success, "导入设备操作过程发生异常");
            }

            //todo:导入后重新执行工具

            return result;
        }

        public async Task<JsonDataMsg<ExcelPackage>> ExportMachinesAsync()
        {
            //先尝试获取100条数据
            int current = 1;
            int capacity = 100;
            DataBoxDto<EquipmentDto> dataBoxDto = await Repository.QuantitativeTargetsAsync(current, capacity);
            if (0 == dataBoxDto.Total)
            {
                //如果没有数据，则导出一个空的Excel模板文件，并返回
                ExcelPackage emptyExcelPackage = ExcelOptr.CreateExcel(new ExcelConfig()
                {
                    CreateWorksheetCallBack = (worksheet) =>
                    {
                        //定义头
                        worksheet.Cells[1, 1].Value = "ip";
                        worksheet.Cells[1, 2].Value = "备注";
                    }
                });

                if (null == emptyExcelPackage)
                {
                    NLogMgr.ErrorExLog(_errorLog, "创建Excel失败", null);
                    return new JsonDataMsg<ExcelPackage>(null, false, "创建Excel失败");
                }

                NLogMgr.DebugLog(_programLog, "创建Excel模板成功");
                return new JsonDataMsg<ExcelPackage>(emptyExcelPackage, true, "");
            }

            //有数据则根据数据，得到总分页数，便于后续遍历
            var totalPage = (int)Math.Ceiling(dataBoxDto.Total * 1.0 / capacity);
            List<EquipmentDto> eqList = new List<EquipmentDto>(dataBoxDto.Data);
            if (totalPage > current)
            {
                //还需要获取后面分页的数据
                while (++current <= totalPage)
                {
                    dataBoxDto = await Repository.QuantitativeTargetsAsync(current, capacity);
                    if ((null == dataBoxDto.Data || !dataBoxDto.Data.Any()) && (current <= totalPage))
                    {
                        NLogMgr.ErrorExLog(_errorLog, $"导出第{current}页数据的时候出错了", null);
                        return new JsonDataMsg<ExcelPackage>(null, false, $"导出第{current}页数据的时候出错了");
                    }

                    if (null != dataBoxDto.Data && dataBoxDto.Data.Any())
                    {
                        eqList.AddRange(dataBoxDto.Data);
                    }
                }

            }

            //取完剩余的分页数据，则可以开始生成Excel文件
            //todo:生成Excel文件
            ExcelPackage excelPackage = ExcelOptr.CreateExcel(new ExcelConfig()
            {
                CreateWorksheetCallBack = (worksheet) =>
                {
                    //定义头
                    worksheet.Cells[1, 1].Value = "ip";
                    worksheet.Cells[1, 2].Value = "备注";

                    for (int i = 0; i < eqList.Count; i++)
                    {
                        worksheet.Cells[i + 2, 1].Value = eqList[i].IpAddressV4;
                        worksheet.Cells[i + 2, 2].Value = eqList[i].Remarks ?? string.Empty;
                    }
                }
            });

            if (null == excelPackage)
            {
                NLogMgr.ErrorExLog(_errorLog, "创建Excel失败", null);
                return new JsonDataMsg<ExcelPackage>(null, false, $"创建Excel失败");
            }

            NLogMgr.DebugLog(_programLog, "数据存储到内存Excel成功");
            return new JsonDataMsg<ExcelPackage>(excelPackage, true, "");
        }
    }
}
