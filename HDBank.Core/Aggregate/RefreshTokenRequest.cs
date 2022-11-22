namespace HDBank.Core.Aggregate
{
    public class RefreshTokenRequest
    {
        public string ClientId { get; set; } = string.Empty;
        public string GrantType { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}