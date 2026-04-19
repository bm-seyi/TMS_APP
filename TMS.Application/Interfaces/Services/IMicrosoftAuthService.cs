using TMS.Domain;

namespace TMS.Application.Interfaces.Services
{
    public interface IMicrosoftAuthService
    {
        Task<AuthenticatedUser> LoginAsync(CancellationToken cancellationToken);
    }
}