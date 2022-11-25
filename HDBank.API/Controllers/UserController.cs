using HDBank.API.Models;
using HDBank.Core.Aggregate;
using HDBank.Core.Aggregate.ChangePassword;
using HDBank.Core.Aggregate.Login;
using HDBank.Core.Aggregate.Register;
using HDBank.Core.Aggregate.Tranfer;
using HDBank.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

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
        public async Task<IActionResult> Login(LoginModel request)
        {
            var key = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCDY1DzbqoavP8UVPYARHpy+zPlaFiBdf3imr5m4RdbHCwMueevk+NoWV2dqL/LBnk8oWMqWkgMDnTleXe/jvj6zQEuuCoBVDiZq4k0JXbHdTmXg0/fH7d9YD0BsSkpSJH8A9RBSnjvIzKLNHXKTUyxG1QIIKbU2lhVAB/jK2UtdwIDAQAB";
            LoginData loginData = new()
            {
                UserName = request.UserName,
                Password = request.Password
            };
            BankRequest<LoginRequestData> bankRequest = new();
            bankRequest.Data = new LoginRequestData()
            {
                Credential = _service.GenerateCredential(loginData, key),
                Key = key
            };

            var response = await _service.Login(bankRequest);
            if (response.Response.ResponseCode == "00")
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Response.ResponseMessage);
        }
        // TODO: request contain: credential{username, password}, email, number, phone

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel request)
        {
            var key = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCDY1DzbqoavP8UVPYARHpy+zPlaFiBdf3imr5m4RdbHCwMueevk+NoWV2dqL/LBnk8oWMqWkgMDnTleXe/jvj6zQEuuCoBVDiZq4k0JXbHdTmXg0/fH7d9YD0BsSkpSJH8A9RBSnjvIzKLNHXKTUyxG1QIIKbU2lhVAB/jK2UtdwIDAQAB";
            BankRequest<RegisterRequestData> bankRequest = new();
            RegisterData registerData = new()
            {
                UserName = request.UserName,
                Password = request.Password
            };
            bankRequest.Data = new RegisterRequestData()
            {
                Credential = _service.GenerateCredential(registerData, key),
                FullName = request.FullName,
                Email = request.Email,
                IdentityNumber = request.IdentityNumber,
                Key = key,
                Phone = request.Phone
            };

            var response = await _service.Register(bankRequest);
            if (response.Response.ResponseCode == "00")
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Response.ResponseMessage);
        }
        // TODO: request contain: credential{username, old password, new password}, 
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordModel request)
        {
            var key = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCDY1DzbqoavP8UVPYARHpy+zPlaFiBdf3imr5m4RdbHCwMueevk+NoWV2dqL/LBnk8oWMqWkgMDnTleXe/jvj6zQEuuCoBVDiZq4k0JXbHdTmXg0/fH7d9YD0BsSkpSJH8A9RBSnjvIzKLNHXKTUyxG1QIIKbU2lhVAB/jK2UtdwIDAQAB";
            ChangePasswordData changePasswordModel = new()
            {
                UserName = request.UserName,
                OldPassword = request.OldPassword,
                NewPassword = request.NewPassword
            };
            BankRequest<ChangePasswordRequestData> bankRequest = new();
            bankRequest.Data = new ChangePasswordRequestData()
            {
                Credential = _service.GenerateCredential(changePasswordModel, key),
                Key = key
            };

            var response = await _service.ChangePassword(bankRequest);
            if (response.Response.ResponseCode == "00")
            {
                return Ok(response.Response.ResponseMessage);
            }
            return BadRequest(response.Response.ResponseMessage);
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
        // 
        // TODO: TRANFER(NHON) request contain: ?description, amount, to account number
        // response contain: 
        [HttpPost("tranfer")]
        public async Task<IActionResult> Tranfer(TransferRequestData request)
        {
            BankRequest<TransferRequestData> bankRequest = new();
            bankRequest.Data = new TransferRequestData()
            {
                Amount = request.Amount,
                Description = request.Description,
                FromAccount = request.FromAccount,
                ToAccount = request.ToAccount
            };
            var response = await _service.Tranfer(bankRequest);
            if (response.Response.ResponseCode == "00")
            {
                return Ok(response.Response.ResponseMessage);
            }
            return BadRequest(response.Response.ResponseMessage);
        }
        // TODO: request contain: 
        // response contain:

    }
}
