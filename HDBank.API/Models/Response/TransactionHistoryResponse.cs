using Newtonsoft.Json;

namespace HDBank.API.Models.Response
{
    public class TransactionHistoryResponse
    {
        [JsonProperty("history")]
        public IEnumerable<TransactionHistory> Histories;
    }
    public class TransactionHistory
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int Amount { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
    }
}
