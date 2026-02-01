using TMS.Models.PipelineContexts;

namespace TMS.Core.Interfaces.Services
{
    public interface ILoginService
    {
        Task<bool> LoginAsync(LoginContext loginContext, CancellationToken cancellationToken);
    }
}