using Newtonsoft.Json;

namespace HDBank.Core.Aggregate
{
    public class ResponseModel
    {
        [JsonProperty("responseId")]
        public string ResponseId { get; set; }
        [JsonProperty("responseCode")]
        public string ResponseCode { get; set; }
        [JsonProperty("responseMessage")]
        public string ResponseMessage { get; set; }
        [JsonProperty("responseTime")]
        public string ResponseTime { get; set; }
    }
}