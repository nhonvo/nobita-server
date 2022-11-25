using HDBank.Infrastructure.Models;
using System.Security.Claims;

namespace HDBank.Infrastructure.Services
{
    public interface IJwtManager
    {
        public string Authenticate(AppUser user, IList<string> roles);
        public ClaimsPrincipal Validate(string token);
        public DateTime GetExpireDate(string token);
    }
}