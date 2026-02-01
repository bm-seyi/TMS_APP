using Microsoft.Identity.Client;

namespace TMS.Core.Interfaces.Services
{
    public interface IMicrosoftAuthService
    {
        Task<AuthenticationResult> LoginAsync(CancellationToken cancellationToken);
    }
}