using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManjuuDomain.Dto;
using ManjuuDomain.HealthCheck;
using ManjuuDomain.IDomain;
using Microsoft.AspNetCore.Mvc;

namespace ManjuuManagement.Controllers
{
    public class ToolController : Controller
    {
        private ICheckConfigRepository CheckConfigRepository;

        public ToolController(ICheckConfigRepository checkConfigRepository)
        {
            CheckConfigRepository = checkConfigRepository;
        }
        public async Task<IActionResult> Index()
        {
            List<ToolConfigDto> list = await CheckConfigRepository.GetValidConfigsAsync();
            return View(list);
        }

        [HttpPost]
        public IActionResult Add(ToolConfigDto newConfiguration)
        {
            if (null == newConfiguration)
            {

            }
            
            return null;
        }
    }
}