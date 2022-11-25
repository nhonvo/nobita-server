using HDBank.API.Models;
using HDBank.API.Services;
using HDBank.Core.Aggregate;
using HDBank.Core.Aggregate.AppResult;
using HDBank.Core.Aggregate.Balance;
using HDBank.Core.Aggregate.ChangePassword;
using HDBank.Core.Aggregate.Login;
using HDBank.Core.Aggregate.Register;
using HDBank.Core.Aggregate.Tranfer;
using HDBank.Core.Aggregate.TranferHistory;
using HDBank.Core.Interfaces;
using HDBank.Infrastructure.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HDBank.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAPIService _service;
        private readonly IAppService _appService;

        public UserController(IAPIService service, IAppService appService)
        {
            _service = service;
            _appService = appService;
        }
        public string key = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCDY1DzbqoavP8UVPYARHpy+zPlaFiBdf3imr5m4RdbHCwMueevk+NoWV2dqL/LBnk8oWMqWkgMDnTleXe/jvj6zQEuuCoBVDiZq4k0JXbHdTmXg0/fH7d9YD0BsSkpSJH8A9RBSnjvIzKLNHXKTUyxG1QIIKbU2lhVAB/jK2UtdwIDAQAB";
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel request)
        {
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
            if (response.Response.ResponseCode != "00")
            {
                return BadRequest(new ApiErrorResult<string>(response.Response.ResponseMessage));
            }
            var appResponse = await _appService.Authenticate(request);
            if (appResponse.Succeeded)
            {
                return Ok(appResponse);
            }
            return BadRequest(appResponse);

        }
        // TODO: request contain: credential{username, password}, email, number, phone

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel request)
        {
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
            if (response.Response.ResponseCode != "00")
            {
                return BadRequest(new ApiErrorResult<string>(response.Response.ResponseMessage));
            }

            // Login to add AccNo
            LoginData loginData = new()
            {
                UserName = request.UserName,
                Password = request.Password
            };

            BankRequest<LoginRequestData> bankLoginRequest = new();
            bankLoginRequest.Data = new LoginRequestData()
            {
                Credential = _service.GenerateCredential(loginData, key),
                Key = key
            };

            var loginResponse = await _service.Login(bankLoginRequest);

            if (loginResponse.Response.ResponseCode != "00")
            {
                return BadRequest(new ApiErrorResult<string>(loginResponse.Response.ResponseMessage));
            }
            var appResponse = await _appService.Register(request, loginResponse.Data.AccountNo);
            if (appResponse.Succeeded)
            {
                return Ok(appResponse);
            }
            return BadRequest(appResponse);
        }
        [Authorize]
        [HttpGet("get-info")]
        public async Task<IActionResult> GetInfo()
        {
            var response = await _appService.GetByClaims(User);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        // TODO: request contain: credential{username, old password, new password}, 
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordModel request)
        {
            ChangePasswordData changePasswordData = new()
            {
                UserName = request.UserName,
                OldPassword = request.OldPassword,
                NewPassword = request.NewPassword
            };
            BankRequest<ChangePasswordRequestData> bankRequest = new();
            bankRequest.Data = new ChangePasswordRequestData()
            {
                Credential = _service.GenerateCredential(changePasswordData, key),
                Key = key
            };

            var response = await _service.ChangePassword(bankRequest);
            if (response.Response.ResponseCode == "00")
            {
                return Ok(response.Response.ResponseMessage);
            }
            return BadRequest(response.Response.ResponseMessage);
        }
        [Authorize]
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
        public async Task<IActionResult> Tranfer(TransferModel request)
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
        // TODO: request contain: Get trafer history
        // response contain:
        [HttpPost("get-transfer-history")]
        public async Task<IActionResult> GetTransferHistory(TranferHistoryModel request)
        {
            BankRequest<TranferHistoryRequestData> bankRequest = new();
            bankRequest.Data = new TranferHistoryRequestData()
            {
                AccountNumber = request.AccountNumber,
                FromDate = request.FromDate,
                ToDate = request.ToDate
            };
            var response = await _service.TranferHistory(bankRequest);
            if (response.Response.ResponseCode == "00")
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Response.ResponseMessage);
        }
        // Bug: post but in swagger is get
        [HttpPost("balance")]
        public async Task<IActionResult> Balance(BalanceModel request)
        {
            BankRequest<BalanceRequestData> bankRequest = new();
            bankRequest.Data = new BalanceRequestData()
            {
                AccountNumber = request.AccountNumber
            };
            var response = await _service.Balance(bankRequest);
            if (response.Response.ResponseCode == "00")
            {
                return Ok(response.Data.Amount);
            }
            return BadRequest(response.Response.ResponseMessage);
        }
    }
}
