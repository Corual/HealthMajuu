using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManjuuApplications;
using ManjuuCommon.DataPackages;
using ManjuuCommon.ILog;
using ManjuuDomain.Dto;
using ManjuuDomain.IDomain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace ManjuuManagement.Controllers
{
    public class MachineController : Controller
    {
        //ICheckTargetRepository _repository;
        IMachineApplication _application;

        //public MachineController(ICheckTargetRepository repository)
        //{
        //    _repository = repository;
        //}

        public MachineController(IMachineApplication machineApplication)
        {
            _application = machineApplication;
        }

        public async Task<IActionResult> Index()
        {
            //MachineApplication _application = new MachineApplication(_repository);

            PageMsg<EquipmentDto> pageMsg = await _application.PaggingMachinesAsync(1);
            return View(pageMsg);


        }

        [HttpPost]
        public async Task<IActionResult> Paging(int id)
        {
            //MachineApplication _application = new MachineApplication(_repository);

            PageMsg<EquipmentDto> pageMsg = await _application.PaggingMachinesAsync(id);
            return Json(pageMsg);
        }

        [HttpPost]
        public async Task<IActionResult> Import(IFormCollection  formCollection)
        {
            //用axios库，不能用IFormFile接收，改用IFormCollection

            //MachineApplication _application = new MachineApplication(_repository);

            return Json(await _application.ImportingMachinesAsync(formCollection));
        }


        [HttpPost]
        public async Task<IActionResult> Export(IFormFile formFile)
        {

            //MachineApplication _application = new MachineApplication(_repository);

            var result = await _application.ExportMachinesAsync();


            if (!result.BusinessResult)
            {
                return Json(result);
            }

            string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(result.ResponseData.GetAsByteArray(), XlsxContentType, "report.xlsx");

           
        }


    }
}