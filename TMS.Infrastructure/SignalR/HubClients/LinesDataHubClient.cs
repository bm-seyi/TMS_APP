using System.Diagnostics;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TMS.Application.Extensions;
using TMS.Domain.DTOs;

namespace TMS.Infrastructure.SignalR.HubClients;

internal sealed class LinesDataHubClient(ILogger<LinesDataHubClient> logger, IConfiguration configuration) : HubClientBase(logger, _activitySource) 
{
    private readonly ILogger<LinesDataHubClient> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    private static readonly ActivitySource _activitySource = new ActivitySource("TMS.Infrastructure");

    protected override string HubUrl => $"{_configuration.GetRequiredValue<string>("SignalR:Hub")}/linesDataHub";

    public event Action<LinesUpdateDTO>? LineReceived;

    protected override void RegisterHandlers(HubConnection connection)
    {
        using Activity? _ = _activitySource.StartActivity("LinesDataHubClient.RegisterHandlers");

        connection.On<LinesUpdateDTO>("ReceiveLines", line =>
        {
            LineReceived?.Invoke(line);
        });
    }
}
