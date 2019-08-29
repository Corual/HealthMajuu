using ManjuuCommon.Tools;
using ManjuuDomain.Dto;
using ManjuuDomain.FileAssist;
using ManjuuDomain.IDomain;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManjuuDomain.FileAssistService
{
    public class ExcelFileService
    {
        public async static Task<List<EquipmentDto>> GetExcelDataAsync(IFormFile file)
        {
            List<EquipmentDto> list = null;
            ExcelFile excelFile = null;
            using (excelFile = await ExcelFile.CreateFormFormFileAsync(file))
            {

                list = excelFile.GetExcelData<EquipmentDto>(
                    new ExcelMapper[] { new ExcelMapper("ip",nameof(EquipmentDto.IpAddressV4)),
                     new ExcelMapper("备注",nameof(EquipmentDto.Remarks))});
            }

            return list;

        }


        public static ExcelPackage CreateEquipmentTemplate()
        {
           return  ExcelOptr.CreateExcel(new ExcelConfig()
            {
                CreateWorksheetCallBack = (worksheet) =>
                {
                    //定义头
                    worksheet.Cells[1, 1].Value = "ip";
                    worksheet.Cells[1, 2].Value = "备注";
                }
            });
        }


        public static ExcelPackage CreateEquipmentExcel(List<EquipmentDto> eqList)
        {
           return ExcelOptr.CreateExcel(new ExcelConfig()
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
        }



    }
}
