using HDBank.Core.Aggregate;
using HDBank.Core.Aggregate.ChangePassword;
using HDBank.Core.Aggregate.GetKey;
using HDBank.Core.Aggregate.Login;
using HDBank.Core.Aggregate.RefreshToken;
<<<<<<< HEAD
using HDBank.Core.Aggregate.Tranfer;
=======
using HDBank.Core.Aggregate.Register;
>>>>>>> b838be29b92619ddbb0ce5d7e3ae12915e7e2ea7
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

    }
}