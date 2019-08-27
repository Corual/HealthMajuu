using ManjuuCommon.DataPackages;
using ManjuuDomain.Dto;
using ManjuuDomain.IDomain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ManjuuApplications
{
    public class MachineApplication
    {
        private ICheckTargetRepository Repository;
        public MachineApplication(ICheckTargetRepository repository)
        {
            Repository = repository;
        }

        public async Task<PageMsg<EquipmentDto>> PaggingMachinesAsync(int current)
        {
            if (current < 1)
            {
                current = 1;
            }

            int capacity = 20; //每页20条
            DataBoxDto<EquipmentDto> dataBoxDto = await Repository.QuantitativeTargetsAsync(current, capacity);
            if (0 == dataBoxDto.Total)
            {
                return new PageMsg<EquipmentDto>() { Msg="获取数据异常或者没数据", BusinessResult=false};
            }

            PageMsg<EquipmentDto> pageMsg = new PageMsg<EquipmentDto>(dataBoxDto.Data, current, dataBoxDto.Total, capacity);
            pageMsg.Msg = "获取成功";
            pageMsg.BusinessResult = true;
            return pageMsg;
        }

        public async Task<PageMsg<string>> ImportingMachinesAsync(IFormFile formFile)
        {
            #region IFormFile
            //public interface IFormFile
            //{
            //    //
            //    // 摘要:
            //    //     Gets the raw Content-Type header of the uploaded file.
            //    string ContentType { get; }
            //    //
            //    // 摘要:
            //    //     Gets the raw Content-Disposition header of the uploaded file.
            //    string ContentDisposition { get; }
            //    //
            //    // 摘要:
            //    //     Gets the header dictionary of the uploaded file.
            //    IHeaderDictionary Headers { get; }
            //    //
            //    // 摘要:
            //    //     Gets the file length in bytes.
            //    long Length { get; }
            //    //
            //    // 摘要:
            //    //     Gets the form field name from the Content-Disposition header.
            //    string Name { get; }
            //    //
            //    // 摘要:
            //    //     Gets the file name from the Content-Disposition header.
            //    string FileName { get; }

            //    //
            //    // 摘要:
            //    //     Copies the contents of the uploaded file to the target stream.
            //    //
            //    // 参数:
            //    //   target:
            //    //     The stream to copy the file contents to.
            //    void CopyTo(Stream target);
            //    //
            //    // 摘要:
            //    //     Asynchronously copies the contents of the uploaded file to the target stream.
            //    //
            //    // 参数:
            //    //   target:
            //    //     The stream to copy the file contents to.
            //    //
            //    //   cancellationToken:
            //    Task CopyToAsync(Stream target, CancellationToken cancellationToken = default);
            //    //
            //    // 摘要:
            //    //     Opens the request stream for reading the uploaded file.
            //    Stream OpenReadStream();
            //}
            #endregion



            return null;
        }

        public async Task<PageMsg<string>> ExportMachinesAsync()
        {

            return null;
        }
    }
}
