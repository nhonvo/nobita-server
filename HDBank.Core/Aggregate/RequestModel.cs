using Newtonsoft.Json;
using System;

namespace HDBank.Core.Aggregate
{
    public class RequestModel
    {
        [JsonProperty("requestId")]
        public string RequestId { get; set; } = Guid.NewGuid().ToString();
        [JsonProperty("requestTime")]
        public string RequestTime { get; set; } = DateTime.Now.ToString();
    }
}