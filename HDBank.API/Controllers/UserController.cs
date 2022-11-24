using HDBank.Core.Aggregate;
using HDBank.Core.Aggregate.Login;
using HDBank.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HDBank.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAPIService _service;

        public UserController(IAPIService service)
        {
            _service = service;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginData request)
        {
            var key = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCDY1DzbqoavP8UVPYARHpy+zPlaFiBdf3imr5m4RdbHCwMueevk+NoWV2dqL/LBnk8oWMqWkgMDnTleXe/jvj6zQEuuCoBVDiZq4k0JXbHdTmXg0/fH7d9YD0BsSkpSJH8A9RBSnjvIzKLNHXKTUyxG1QIIKbU2lhVAB/jK2UtdwIDAQAB";
            BankRequest<LoginRequest> bankRequest = new BankRequest<LoginRequest>();
            bankRequest.Data = new LoginRequest()
            {
                Credential = _service.GenerateCredential(request, key),
                Key = key
            };

            var response = await _service.Login(bankRequest);
            if (response.Response.ResponseCode == "00")
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Data);
        }
        // TODO: request contain: credential{username, password}, email, number, phone

        [HttpPost("register")]
        public IActionResult Register()
        {
            return Ok();
        }
        // TODO: request contain: credential{username, old password, new password}, 
        [HttpPost("change-password")]
        public IActionResult ChangePassword()
        {
            return Ok();
        }
        // TODO: request contain: account number
        // reponse contain: amount
        [HttpGet("balance")]
        public IActionResult Balance()
        {
            return Ok();
        }
        [HttpGet("get-access-token")]
        public async Task<IActionResult> GetAccessToken()
        {
            var response = await _service.RefeshToken();
            return Ok(response.AccessToken);
        }
        // TODO: request contain: ?description, amount, to account number
        // response contain: 
        // TODO: request contain: 
        // response contain:
        
    }
}
