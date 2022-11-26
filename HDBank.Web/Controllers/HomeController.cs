using HDBank.Core.Aggregate;
using HDBank.Core.Aggregate.Login;
using HDBank.Core.Interfaces;
using HDBank.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading.Tasks;

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
            return View("Index", result.Data.Key);
        }
        [HttpGet]
        public IActionResult Login()
        {
            LoginModel model = new()
            {
                ReturnUrl = ""
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            LoginData data = new()
            {
                UserName = model.UserName,
                Password = model.Password
            };
            var key = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCDY1DzbqoavP8UVPYARHpy+zPlaFiBdf3imr5m4RdbHCwMueevk+NoWV2dqL/LBnk8oWMqWkgMDnTleXe/jvj6zQEuuCoBVDiZq4k0JXbHdTmXg0/fH7d9YD0BsSkpSJH8A9RBSnjvIzKLNHXKTUyxG1QIIKbU2lhVAB/jK2UtdwIDAQAB";
            var credential = _service.GenerateCredential(data, key);
            var request = new BankRequest<LoginRequestData>();
            // Encrypt to credential
            request.Data = new LoginRequestData();
            request.Data.Credential = credential;
            request.Data.Key = key;
            request.Request = new RequestModel();
            Console.WriteLine("Time Now : " + DateTime.Now.ToString());

            var result = await _service.Login(request);
            if (result.Response.ResponseCode == "00")
                return Content(result.Data.AccountNo);
            return RedirectToAction(nameof(Index));
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