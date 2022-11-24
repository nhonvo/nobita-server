using Newtonsoft.Json;

namespace HDBank.Core.Aggregate
{
    public class LoginData
    {
        [JsonProperty("username")]
        public string UserName { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}