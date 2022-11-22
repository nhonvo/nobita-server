using HDBank.Core.Interfaces;
using HDBank.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HDBank.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAPIService _service;

        public HomeController(ILogger<HomeController> logger, IAPIService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _service.GetKey();
            return View("Index", result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}