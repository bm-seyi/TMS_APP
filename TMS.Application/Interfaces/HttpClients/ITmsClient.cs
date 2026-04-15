using TMS.Models;

namespace TMS.Core.Interfaces.HttpClients
{
    public interface ITmsClient
    {
        Task<ArcgisSecret> GetArcgisApiKeyAsync(CancellationToken cancellationToken);
    }
}