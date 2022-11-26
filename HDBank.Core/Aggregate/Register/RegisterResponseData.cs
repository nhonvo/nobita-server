using Newtonsoft.Json;

namespace HDBank.Core.Aggregate.Register
{
    public class RegisterResponseData
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }
    }
}