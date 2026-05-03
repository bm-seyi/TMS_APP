using System.Diagnostics;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace TMS.Infrastructure.SignalR;

internal abstract class HubClientBase(ILogger logger, ActivitySource activitySource) : IAsyncDisposable
{
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly ActivitySource _activitySource = activitySource ?? throw new ArgumentNullException(nameof(activitySource));

    protected HubConnection? Connection;

    protected abstract string HubUrl { get; }

    public bool IsConnected => Connection?.State == HubConnectionState.Connected;

    public async Task ConnectAsync()
    {
        using Activity? _ = _activitySource.StartActivity("HubClientBase.ConnectAsync");

        if (Connection != null && IsConnected)
            return;

        Connection = new HubConnectionBuilder()
            .WithUrl(HubUrl)
            .WithAutomaticReconnect()
            .Build();

        RegisterHandlers(Connection);

        await Connection.StartAsync();
    }

    public async Task DisconnectAsync()
    {
        using Activity? _ = _activitySource.StartActivity("HubClientBase.DisconnectAsync");

        if (Connection == null)
            return;

        if (Connection.State != HubConnectionState.Disconnected)
            await Connection.StopAsync();
    }

    protected abstract void RegisterHandlers(HubConnection connection);

    public async ValueTask DisposeAsync()
    {
        using Activity? _ = _activitySource.StartActivity("HubClientBase.DisposeAsync");
        if (Connection != null)
        {
            await DisconnectAsync();
            await Connection.DisposeAsync();
            Connection = null;
        }
        GC.SuppressFinalize(this);
    }
}