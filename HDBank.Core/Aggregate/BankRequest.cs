using Newtonsoft.Json;

namespace HDBank.Core.Aggregate
{
    public class BankRequest<DataType>
    {
        [JsonProperty("data")]
        public DataType Data { get; set; }
        [JsonProperty("request")]
        public RequestModel Request { get; set; } = new RequestModel();
    }
}