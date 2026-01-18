using System.Diagnostics;
using Microsoft.Extensions.Logging;
using TMS.Core.Interfaces.HttpClients;
using TMS.Core.Interfaces.Services;
using TMS.Models;


namespace TMS.Core.Services
{
    internal sealed class ArcgisService : IArcgisService
    {
        private readonly ILogger<ArcgisService> _logger;
        private readonly ITmsClient _tmsClient;
        private static readonly ActivitySource _activitySource = new ActivitySource("TMS.Core.Services.ArcgisService");

        public ArcgisService(ILogger<ArcgisService> logger, ITmsClient tmsClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tmsClient = tmsClient ?? throw new ArgumentNullException(nameof(tmsClient));
        }

        public async Task RegisterAsync(CancellationToken cancellationToken = default)
        {
            using Activity? activity = _activitySource.StartActivity("ArcgisService.RegisterAsync");

            _logger.LogInformation("Starting ArcGIS registration.");
            
            ArcgisSecret arcgisSecret = await _tmsClient.GetArcgisApiKeyAsync(cancellationToken);
            Esri.ArcGISRuntime.ArcGISRuntimeEnvironment.ApiKey = arcgisSecret.ApiKey;

            _logger.LogInformation("ArcGIS registration completed successfully.");
        }
    }
}