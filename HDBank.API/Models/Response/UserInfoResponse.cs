using Newtonsoft.Json;

namespace HDBank.API.Models.Response
{
    public class UserInfoResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("username")]
        public string UserName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("fullname")]
        public string FullName { get; set; }
        [JsonProperty("acctNo")]
        public string AccountNo { get; set; }
    }
}
