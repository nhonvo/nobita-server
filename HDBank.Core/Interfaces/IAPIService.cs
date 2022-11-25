using HDBank.Core.Aggregate;
using HDBank.Core.Aggregate.Balance;
using HDBank.Core.Aggregate.ChangePassword;
using HDBank.Core.Aggregate.GetKey;
using HDBank.Core.Aggregate.Login;
using HDBank.Core.Aggregate.RefreshToken;
using HDBank.Core.Aggregate.Register;
using HDBank.Core.Aggregate.Tranfer;
using HDBank.Core.Aggregate.TranferHistory;
using System.Threading.Tasks;

namespace HDBank.Core.Interfaces
{
    public interface IAPIService
    {
        Task<BankResponse<GetKeyResponseData>> GetKey();
        Task<BankResponse<LoginResponseData>> Login(BankRequest<LoginRequestData> request);
        Task<BankResponse<RegisterResponseData>> Register(BankRequest<RegisterRequestData> request);
        Task<BankResponse<ChangePasswordResponseData>> ChangePassword(BankRequest<ChangePasswordRequestData> request);
        string GenerateCredential(IData data, string publicKey);
        Task<RefreshTokenResponse> RefeshToken();
        Task<BankResponse<TransferResponseData>> Tranfer(BankRequest<TransferRequestData> request);
        Task<BankResponse<TranferHistoryResponseData>> TranferHistory(BankRequest<TranferHistoryRequestData> request);
        Task<BankResponse<BalanceResponseData>> Balance(BankRequest<BalanceRequestData> request);
    }
}