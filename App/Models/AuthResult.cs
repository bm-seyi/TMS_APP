
namespace TMS_APP.Models
{
    public class AuthResult
    {
        public bool IsError { get; set; }
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public required string IdentityToken { get; set; }
        public string? Error { get; set; }
        public string? ErrorDescription { get; set; }
        public DateTimeOffset AccessTokenExpiration { get; set; }
    }
}