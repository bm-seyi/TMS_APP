using System.Diagnostics;
using Microsoft.Extensions.Logging;
using TMS.Application.Interfaces.HttpClients;
using TMS.Domain;
using TMS.Infrastructure.Extensions;


namespace TMS.Infrastructure.Http.Clients;

internal sealed class TmsClient(ILogger<TmsClient> logger, HttpClient httpClient) : ITmsClient
{
    private readonly ILogger<TmsClient> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    private static readonly ActivitySource _activitySource = new ActivitySource("TMS.Infrastructure");

    public async Task<ArcgisSecret> GetArcgisApiKeyAsync(CancellationToken cancellationToken)
    {
        using Activity? activity = _activitySource.StartActivity("TmsClient.GetArcgisSecretAsync");

        _logger.LogInformation("Requesting ArcGIS API secret from {Endpoint}", "api/v1/secrets/arcgis");

        ArcgisSecret arcgisSecret = await _httpClient.GetRequiredFromJsonAsync<ArcgisSecret>("api/v1/secrets/arcgis", cancellationToken);
    
        _logger.LogInformation("Successfully retrieved ArcGIS API secret");

        return arcgisSecret;
    }
}
