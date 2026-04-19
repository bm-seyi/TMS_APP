namespace TMS.Application.Interfaces.Services
{
    public interface IArcgisService
    {
        Task RegisterAsync(CancellationToken cancellationToken = default);
    }
}