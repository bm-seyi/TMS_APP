using System.Diagnostics;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TMS.Application.Extensions;
using TMS.Domain.DTOs;

namespace TMS.Infrastructure.SignalR.HubClients;

internal sealed class MapHubClient(ILogger<MapHubClient> logger, IConfiguration configuration) : HubClientBase(logger, _activitySource) 
{
    private readonly ILogger<MapHubClient> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    private static readonly ActivitySource _activitySource = new ActivitySource("TMS.Infrastructure");
    protected override string HubUrl => $"{_configuration.GetRequiredValue<string>("SignalR:Hub")}/mapHub";
    
    public event Action<MapLinesDTO>? MapLinesLoaded;

    protected override void RegisterHandlers(HubConnection connection)
    {
        using Activity? _ = _activitySource.StartActivity("LinesDataHubClient.RegisterHandlers");

        connection.On<MapLinesDTO>("MapLinesLoaded", line =>
        {
            MapLinesLoaded?.Invoke(line);
        });
    }
}
