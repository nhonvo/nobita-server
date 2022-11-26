using Newtonsoft.Json;

namespace HDBank.API.Models
{
    public class RegisterModel
    {
        [JsonProperty("username")]
        public string UserName { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("fullName")]
        public string FullName { get; set; }
        [JsonProperty("identityNumber")]
        public string IdentityNumber { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
    }
}
