using Microsoft.Identity.Client;

namespace TMS.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthenticationResult> LoginAsync(CancellationToken cancellationToken);
    }
}