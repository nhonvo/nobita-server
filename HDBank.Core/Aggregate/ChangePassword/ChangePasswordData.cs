using HDBank.Core.Interfaces;
using Newtonsoft.Json;

namespace HDBank.Core.Aggregate.ChangePassword
{
    public class ChangePasswordData : IData
    {
        [JsonProperty("username")]
        public string UserName { get; set; }
        [JsonProperty("oldPass")]
        public string OldPassword { get; set; }
        [JsonProperty("newPass")]
        public string NewPassword { get; set; }
    }
}