using System.Diagnostics;
using Esri.ArcGISRuntime;
using Microsoft.Extensions.Logging;
using TMS.Application.Interfaces.HttpClients;
using TMS.Core.Interfaces.Services;
using TMS.Domain;

namespace TMS.Infrastructure.Services;

internal sealed class ArcgisService(ILogger<ArcgisService> logger, ITmsClient tmsClient) : IArcgisService
{
    private readonly ILogger<ArcgisService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly ITmsClient _tmsClient = tmsClient ?? throw new ArgumentNullException(nameof(tmsClient));
    private static readonly ActivitySource _activitySource = new ActivitySource("TMS.Infrastructure");

    public async Task RegisterAsync(CancellationToken cancellationToken = default)
    {
        using Activity? activity = _activitySource.StartActivity("ArcgisService.RegisterAsync");

        _logger.LogInformation("Starting ArcGIS registration.");
        
        ArcgisSecret arcgisSecret = await _tmsClient.GetArcgisApiKeyAsync(cancellationToken);
        ArcGISRuntimeEnvironment.ApiKey = arcgisSecret.ApiKey;

        _logger.LogInformation("ArcGIS registration completed successfully.");
    }
}
