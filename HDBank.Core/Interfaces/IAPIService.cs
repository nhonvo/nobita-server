using HDBank.Core.Aggregate;
using System.Threading.Tasks;

namespace HDBank.Core.Interfaces
{
    public interface IAPIService
    {
        Task<string> GetKey();
        Task<string> Login(BankRequest<LoginRequest> request);
        string GenerateCredential(LoginData data, string publicKey);
    }
}