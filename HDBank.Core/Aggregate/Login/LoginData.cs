using HDBank.Core.Interfaces;
using Newtonsoft.Json;

namespace HDBank.Core.Aggregate.Login
{
    public class LoginData : IData
    {
        [JsonProperty("username")]
        public string UserName { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}