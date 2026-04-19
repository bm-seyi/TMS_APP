using TMS.Domain.PipelineContexts;

namespace TMS.Application.Interfaces.Services
{
    public interface ILoginService
    {
        Task<bool> LoginAsync(LoginContext loginContext, CancellationToken cancellationToken);
    }
}