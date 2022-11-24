using Newtonsoft.Json;

namespace HDBank.Core.Aggregate.Login
{
    public class LoginResponseData
    {
        [JsonProperty("accountNo")]
        public string AccountNo { get; set; }
    }
}