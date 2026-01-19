using System.Diagnostics;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using TMS.Core.Interfaces.HttpClients;
using TMS.Models;


namespace TMS.Core.HttpClients
{
    internal sealed class TmsClient : ITmsClient
    {
        private readonly ILogger<TmsClient> _logger;
        private readonly HttpClient _httpClient;
        private static readonly ActivitySource _activitySource = new ActivitySource("TMS.Core.HttpClients.TmsClient");
        
        public TmsClient(ILogger<TmsClient> logger, HttpClient httpClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }


        public async Task<ArcgisSecret> GetArcgisApiKeyAsync(CancellationToken cancellationToken)
        {
            using Activity? activity = _activitySource.StartActivity("TmsClient.GetArcgisSecretAsync");

            _logger.LogInformation("Requesting ArcGIS API secret from {Endpoint}", "api/v1/secrets/arcgis");

            ArcgisSecret? arcgisSecret = await _httpClient.GetFromJsonAsync<ArcgisSecret>("api/v1/secrets/arcgis", cancellationToken);
            
            if (arcgisSecret == null)
            {
                _logger.LogWarning("ArcGIS API secret request succeeded but returned null");
                throw new InvalidOperationException();
            }

            _logger.LogInformation("Successfully retrieved ArcGIS API secret");

            return arcgisSecret;
        }
    }
}