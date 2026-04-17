using TMS.Domain;

namespace TMS.Core.Interfaces.Services
{
    public interface IMicrosoftAuthService
    {
        Task<AuthenticatedUser> LoginAsync(CancellationToken cancellationToken);
    }
}