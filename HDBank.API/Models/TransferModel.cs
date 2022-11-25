using Newtonsoft.Json;

namespace HDBank.API.Models
{
    public class TransferModel
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        //[JsonProperty("fromAcct")]
        //public string FromAccount { get; set; }
        [JsonProperty("toAcct")]
        public string ToAccount { get; set; }
    }
}
