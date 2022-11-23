using HDBank.Core.Aggregate;

namespace HDBank.Web.Models
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; } 
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }
}
