using Newtonsoft.Json;

namespace HDBank.API.Models
{
    public class ChangePasswordModel
    {
        [JsonProperty("oldPass")]
        public string OldPassword { get; set; }
        [JsonProperty("newPass")]
        public string NewPassword { get; set; }
    }
}
