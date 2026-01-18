namespace TMS.Core.Interfaces.Services
{
    public interface IArcgisService
    {
        Task RegisterAsync(CancellationToken cancellationToken = default);
    }
}