using HDBank.Core.Interfaces;
using Newtonsoft.Json;

namespace HDBank.Core.Aggregate.Register
{
    public class RegisterData : IData
    {
        [JsonProperty("username")]
        public string UserName { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}