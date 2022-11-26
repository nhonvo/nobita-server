using Newtonsoft.Json;

namespace HDBank.Core.Aggregate.ChangePassword
{
    public class ChangePasswordRequestData
    {
        [JsonProperty("credential")]
        public string Credential { get; set; }
        [JsonProperty("key")]
        public string Key { get; set; }
    }
}