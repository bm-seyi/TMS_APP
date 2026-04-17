using TMS.Domain;
using TMS.Domain.PipelineContexts;
using TMS.Models;

namespace TMS.Core.Interfaces.AuthenticationProviders
{
    public interface IAuthenticationProvider
    {
        AuthenticationProvider AuthenticationProvider { get; }
        Task AuthenticateAsync(LoginContext loginContext, CancellationToken cancellationToken);
    }
}