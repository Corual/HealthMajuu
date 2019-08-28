using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ManjuuManagement.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Net.Http.Headers;

namespace ManjuuManagement.Controllers
{
    public class HomeController : Controller
    {

        private IHostingEnvironment _env;

        public HomeController(IHostingEnvironment env)
        {
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

            var filePath = $"{_env.WebRootPath}/manjuuerrors/500.html";
            return new PhysicalFileResult(filePath, new MediaTypeHeaderValue("text/html"));
        }
    }
}
