using HDBank.Core.Interfaces;
using Newtonsoft.Json;

namespace HDBank.Core.Aggregate.Login
{
    public class LoginRequestData : IData
    {
        [JsonProperty("credential")]
        public string Credential { get; set; }
        [JsonProperty("key")]
        public string Key { get; set; }
    }
}