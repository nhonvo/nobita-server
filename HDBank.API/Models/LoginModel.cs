using Newtonsoft.Json;

namespace HDBank.API.Models
{
    public class LoginModel
    {
        [JsonProperty("username")]
        public string UserName { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
