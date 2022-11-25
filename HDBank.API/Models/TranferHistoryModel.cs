using Newtonsoft.Json;

namespace HDBank.API.Models
{
    public class TranferHistoryModel
    {
        [JsonProperty("acctNo")]
        public string AccountNumber { get; set; }
        [JsonProperty("fromDate")]
        public string FromDate { get; set; }
        [JsonProperty("toDate")]
        public string ToDate { get; set; }
    }
}
