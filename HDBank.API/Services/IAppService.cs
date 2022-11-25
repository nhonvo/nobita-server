using HDBank.API.Models;
using HDBank.Core.Aggregate.AppResult;

namespace HDBank.API.Services
{
    public interface IAppService
    {
        Task<ApiResult<string>> Authenticate(LoginModel request);
        Task<ApiResult<bool>> Register(RegisterModel request);
    }
}
