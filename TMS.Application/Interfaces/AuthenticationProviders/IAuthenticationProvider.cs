using TMS.Domain;
using TMS.Domain.PipelineContexts;

namespace TMS.Application.Interfaces.AuthenticationProviders
{
    public interface IAuthenticationProvider
    {
        AuthenticationProvider AuthenticationProvider { get; }
        Task AuthenticateAsync(LoginContext loginContext, CancellationToken cancellationToken);
    }
}