using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using TMS_APP.Models;

namespace TMS_APP.HubServices
{
    public interface ILinesHubService : IDisposable
    {
        event Action<List<LinesModel>> OnLinesDataReceived;
        Task ConnectAsync(string accessToken);
        Task DisconnectAsync();
        bool IsConnected { get; }
    }

    public class LinesHubService : ILinesHubService
    {
        private HubConnection _hubConnection;
        public event Action<List<LinesModel>> OnLinesDataReceived = delegate {};
        public bool IsConnected => _hubConnection.State == HubConnectionState.Connected;

        private readonly ILogger<LinesHubService> _logger;

        public LinesHubService(ILogger<LinesHubService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _hubConnection = new HubConnectionBuilder().Build();
        }

        public async Task ConnectAsync(string accessToken)
        {
            try
            {
                _hubConnection = new HubConnectionBuilder()
                    .WithUrl("https://localhost:/lineshub", options =>
                    {
                        options.AccessTokenProvider = () => Task.FromResult(accessToken);
                    })
                    .WithAutomaticReconnect([
                        TimeSpan.Zero, // First attempt immediately
                        TimeSpan.FromSeconds(2),
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(10) // Subsequent attempts with delays
                    ])
                    .Build();

                _hubConnection.On<List<LinesModel>>("ReceiveLinesData", (linesData) =>
                {
                    OnLinesDataReceived?.Invoke(linesData);
                });

                _hubConnection.Closed += async (error) =>
                {
                    // Handle reconnection or notify UI about disconnection
                    await Task.Delay(5000);
                    await _hubConnection.StartAsync();
                };

                _hubConnection.Reconnected += (connectionId) =>
                {
                    
                    // Notify UI about reconnection if needed
                    return Task.CompletedTask;
                };

                await _hubConnection.StartAsync();
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"SignalR connection error: {ex.Message}");
                throw;
            }
        }

        public async Task DisconnectAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.StopAsync();
                await _hubConnection.DisposeAsync();
                _hubConnection = null!;
            }
        }

        public void Dispose()
        {
            DisconnectAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}