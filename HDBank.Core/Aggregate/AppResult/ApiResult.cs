namespace HDBank.Core.Aggregate.AppResult
{
    public class ApiResult<T>
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public T ResultObject { get; set; }
    }
}