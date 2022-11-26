using Newtonsoft.Json;

namespace HDBank.API.Models
{
    public class GetInfoByAccountNoModel
    {
        [JsonProperty("acctNo")]
        public string AccountNo { get; set; }
    }
}
