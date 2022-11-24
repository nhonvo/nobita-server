using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDBank.Core.Aggregate.RefreshToken
{
    public class RefreshTokenRequest
    {
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("grant_type")]
        public string GrantType { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

    }
}
