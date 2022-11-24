using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HDBank.Core.Aggregate
{
    public class BankResponse<DataType>
    {
        [JsonProperty("data")]
        public DataType Data { get; set; }
        [JsonProperty("response")]
        public ResponseModel Response { get; set; } = new ResponseModel();
    }
}