using Newtonsoft.Json;

namespace HDBank.API.Models
{
    public class TranferHistoryModel
    {
        [JsonProperty("fromDate")]
        public DateTime FromDate { get; set; }
        [JsonProperty("toDate")]
        public DateTime ToDate { get; set; }
    }
}
