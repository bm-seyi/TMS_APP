using System.Diagnostics;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using TMS.Models;

namespace TMS.Core.HttpClients
{
    internal sealed class TmsClient
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

            ArcgisSecret? arcgisSecret = await _httpClient.GetFromJsonAsync<ArcgisSecret>("api/v1/secrets/arcgis", cancellationToken);
            
            if (arcgisSecret == null)
            {
                throw new InvalidOperationException();
            }

            return arcgisSecret;
        }
    }
}