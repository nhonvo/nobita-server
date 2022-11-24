using HDBank.Core.Aggregate;
using HDBank.Core.Aggregate.GetKey;
using HDBank.Core.Aggregate.Login;
using System.Threading.Tasks;

namespace HDBank.Core.Interfaces
{
    public interface IAPIService
    {
        Task<BankResponse<GetKeyResponseData>> GetKey();
        Task<BankResponse<LoginResponseData>> Login(BankRequest<LoginRequest> request);
        string GenerateCredential(IData data, string publicKey);
    }
}