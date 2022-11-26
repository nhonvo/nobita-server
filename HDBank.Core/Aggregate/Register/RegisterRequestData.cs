using Newtonsoft.Json;

namespace HDBank.Core.Aggregate.Register
{
    public class RegisterRequestData
    {
        [JsonProperty("credential")]
        public string Credential { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("fullName")]
        public string FullName { get; set; }
        [JsonProperty("identityNumber")]
        public string IdentityNumber { get; set; }
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
    }
}