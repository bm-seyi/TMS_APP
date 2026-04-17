
namespace TMS.Domain;

public sealed record AuthenticatedUser
{
    public string? UserId { get; init; }
    public string? Email { get; init; }
    public string? AccessToken { get; init; }
    public DateTime? ExpiresAt { get; init; }
}