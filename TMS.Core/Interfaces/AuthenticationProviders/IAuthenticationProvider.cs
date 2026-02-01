using TMS.Models;
using TMS.Models.PipelineContexts;

namespace TMS.Core.Interfaces.AuthenticationProviders
{
    public interface IAuthenticationProvider
    {
        AuthenticationProvider AuthenticationProvider { get; }
        Task AuthenticateAsync(LoginContext loginContext, CancellationToken cancellationToken);
    }
}