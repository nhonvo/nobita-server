using Newtonsoft.Json;

namespace HDBank.Core.Aggregate
{
    public class LoginRequest
    {
        [JsonProperty("credential")]
        public string Credential { get; set; }
        [JsonProperty("key")]
        public string Key { get; set; }
    }
}