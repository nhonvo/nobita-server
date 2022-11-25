using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HDBank.Core.Aggregate.TranferHistory
{
    public class TranferHistoryRequestData
    {
        [JsonProperty("acctNo")]
        public string AccountNumber { get; set; }
        [JsonProperty("fromDate")]
        public string FromDate { get; set; }
        [JsonProperty("toDate")]
        public string ToDate { get; set; }
    }
}
