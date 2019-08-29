using ManjuuCommon.DataPackages;
using ManjuuDomain.Dto;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManjuuApplications
{
    public interface IMachineApplication: IApplication
    {
         Task<PageMsg<EquipmentDto>> PaggingMachinesAsync(int current, int capacity = 20);

        Task<JsonDataMsg<string>> ImportingMachinesAsync(IFormCollection formCollection);

        Task<JsonDataMsg<ExcelPackage>> ExportMachinesAsync();
    }
}
