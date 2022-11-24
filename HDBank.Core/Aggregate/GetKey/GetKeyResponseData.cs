using Newtonsoft.Json;

namespace HDBank.Core.Aggregate.GetKey
{
    public class GetKeyResponseData
    {
        [JsonProperty("key")]
        public string Key { get; set; }
    }
}