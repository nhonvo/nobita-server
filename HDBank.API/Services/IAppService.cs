using HDBank.API.Models;
using HDBank.API.Models.Response;
using HDBank.Core.Aggregate.AppResult;
using System.Security.Claims;

namespace HDBank.API.Services
{
    public interface IAppService
    {
        Task<ApiResult<string>> Authenticate(LoginModel request);
        Task<ApiResult<string>> Register(RegisterModel request, string accountNo);
        Task<ApiResult<bool>> ChangePassword(ChangePasswordModel request, ClaimsPrincipal claims);
        Task<ApiResult<UserInfoResponse>> GetByClaims(ClaimsPrincipal claims);
        Task<ApiResult<UserInfoResponse>> GetByAccountNo(string accountNo);
        Task<ApiResult<IEnumerable<TransactionHistory>>> GetTransactionHistory(TranferHistoryModel request, string acctNo);
        Task<ApiResult<IEnumerable<TransactionHistory>>> GetAllTransactionHistory(string acctNo);
        Task<ApiResult<bool>> CreateTransaction(TransferModel request, ClaimsPrincipal claims);
    }
}
