using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HDBank.Core.Aggregate.Tranfer
{
    public class TransferRequestData
    {
        [JsonProperty("amount")]
        public string UserName { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("fromAcct")]
        public string FromAccount { get; set; }
        [JsonProperty("toAcct")]
        public string ToAccount { get; set; }
    }
}
