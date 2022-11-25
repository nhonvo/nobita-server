using HDBank.API.Models;
using HDBank.API.Models.Response;
using HDBank.Core.Aggregate.AppResult;
using System.Security.Claims;

namespace HDBank.API.Services
{
    public interface IAppService
    {
        Task<ApiResult<string>> Authenticate(LoginModel request);
        Task<ApiResult<bool>> Register(RegisterModel request, string accountNo);
        Task<ApiResult<UserInfoResponse>> GetByClaims(ClaimsPrincipal claims);
    }
}
