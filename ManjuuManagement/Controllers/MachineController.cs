using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManjuuApplications;
using ManjuuCommon.DataPackages;
using ManjuuDomain.Dto;
using ManjuuDomain.IDomain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManjuuManagement.Controllers
{
    public class MachineController : Controller
    {
        ICheckTargetRepository Repository;
        public MachineController(ICheckTargetRepository repository)
        {
            Repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            MachineApplication application = new MachineApplication(Repository);

            PageMsg<EquipmentDto> pageMsg = await application.PaggingMachinesAsync(1);
            return View(pageMsg);
        }

        [HttpPost]
        public async Task<IActionResult> Paging(int id)
        {
            MachineApplication application = new MachineApplication(Repository);

            PageMsg<EquipmentDto> pageMsg = await application.PaggingMachinesAsync(id);
            return Json(pageMsg);
        }

        [HttpPost]
        public async Task<IActionResult> Import(IFormFile formFile)
        {

            MachineApplication application = new MachineApplication(Repository);

            return Json(await application.ImportingMachinesAsync(formFile));
        }


        [HttpPost]
        public async Task<IActionResult> Export(IFormFile formFile)
        {

            MachineApplication application = new MachineApplication(Repository);

            return Json(await application.ExportMachinesAsync());
        }


    }
}