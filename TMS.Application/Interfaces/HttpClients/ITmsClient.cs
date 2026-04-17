using TMS.Domain;

namespace TMS.Application.Interfaces.HttpClients
{
    public interface ITmsClient
    {
        Task<ArcgisSecret> GetArcgisApiKeyAsync(CancellationToken cancellationToken);
    }
}