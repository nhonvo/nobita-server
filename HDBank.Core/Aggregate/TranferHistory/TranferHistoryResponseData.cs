using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HDBank.Core.Aggregate.TranferHistory
{
    public class TranferHistoryResponseData
    {
        public List<TransHi> transHis { get; set; }

    }
    public class TransHi
    {
        [JsonProperty("transDesc")]
        public string TranferDescription { get; set; }
        [JsonProperty("transDate")]
        public string TranferDate { get; set; }
        [JsonProperty("transAmount")]
        public string TranferAmount { get; set; }
    }
}
