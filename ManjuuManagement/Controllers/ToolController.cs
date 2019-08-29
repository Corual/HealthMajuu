using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ManjuuApplications;
using ManjuuCommon.DataPackages;
using ManjuuDomain.Dto;
using ManjuuDomain.HealthCheck;
using ManjuuDomain.IDomain;
using ManjuuInfrastructure.Repository.Entity;
using ManjuuInfrastructure.Repository.Mapper.Auto;
using Microsoft.AspNetCore.Mvc;

namespace ManjuuManagement.Controllers
{
    public class ToolController : Controller
    {
        private ICheckConfigRepository _repository;

        IToolConfigApplication _application;

        //public ToolController(ICheckConfigRepository checkConfigRepository)
        //{
        //    CheckConfigRepository = checkConfigRepository;
        //}

        public ToolController(ICheckConfigRepository repository, IToolConfigApplication application)
        {
            _repository = repository;
            _application = application;
        }
        public async Task<IActionResult> Index()
        {

            List<ToolConfigDto> list = await _repository.GetValidConfigsAsync();
            ToolConfigDto result = null;
            if (list != null && list.Any())
            {
                result = list[0];
            }
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ToolConfigDto newConfiguration)
        {
            //var _application = new ToolConfigutaionApplication(CheckConfigRepository);
            
            return Json(await _application.UserAddConfigurationToToolAsync(newConfiguration));
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] ToolConfigDto newConfiguration)
        {
            //var _application = new ToolConfigutaionApplication(CheckConfigRepository);

            return Json(await _application.UserAlterConfigurationToToolAsync(newConfiguration));
        }

    }
}