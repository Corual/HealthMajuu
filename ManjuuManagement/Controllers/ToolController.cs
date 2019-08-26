using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            List<CheckConfig> list = await  CheckConfigRepository.GetValidConfigsAsync();
            return View();
        }
    }
}