using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HDBank.Core.Aggregate.Balance
{
    public class BalanceResponseData
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }
    }
}
