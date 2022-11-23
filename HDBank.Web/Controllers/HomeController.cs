﻿using HDBank.Core.Aggregate;
using HDBank.Core.Interfaces;
using HDBank.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Cryptography;

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
            Console.WriteLine(model.UserName);
            LoginData data = new()
            {
                UserName = model.UserName,
                Password = model.Password
            };
            var key = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCDY1DzbqoavP8UVPYARHpy+zPlaFiBdf3imr5m4RdbHCwMueevk+NoWV2dqL/LBnk8oWMqWkgMDnTleXe/jvj6zQEuuCoBVDiZq4k0JXbHdTmXg0/fH7d9YD0BsSkpSJH8A9RBSnjvIzKLNHXKTUyxG1QIIKbU2lhVAB/jK2UtdwIDAQAB";
            var credential = _service.GenerateCredential(data, key);
            return Content(credential);
            Console.WriteLine("Credential" + credential);
            var request = new BankRequest<LoginRequest>();
            // Encrypt to credential
            var csp = new RSACryptoServiceProvider(2048);
            request.Data = new LoginRequest();
            request.Data.Credential = credential;
            request.Data.Key = key;

            var result = await _service.Login(request);
            Console.WriteLine(result);
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