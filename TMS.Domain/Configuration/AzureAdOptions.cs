
namespace TMS.Domain.Configuration
{
    public record AzureAdOptions
    {
        public required string RedirectUri { get; init; }
        public required string ClientId { get; init; }
        public required string[] Scopes { get; init; }
    }
}