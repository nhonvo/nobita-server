namespace HDBank.Core.Aggregate
{
    public class BankRequest<DataType>
    {
        public DataType? Data { get; set; }
        public RequestModel Request { get; set; } = new RequestModel();
    }
}