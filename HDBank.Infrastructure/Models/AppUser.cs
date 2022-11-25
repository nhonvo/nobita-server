using Microsoft.AspNetCore.Identity;

namespace HDBank.Infrastructure.Models
{
    public class AppUser : IdentityUser
    {
        [PersonalData]
        public string AccountNo { get; set; }
        [PersonalData]
        public string IdentityNumber { get; set; }
        [PersonalData]
        public string FullName { get; set; }
    }
}