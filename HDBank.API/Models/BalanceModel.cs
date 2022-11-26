using Newtonsoft.Json;

namespace HDBank.API.Models
{
    public class BalanceModel
    {
        [JsonProperty("acctNo")]
        public string AccountNumber { get; set; }
    }
}
