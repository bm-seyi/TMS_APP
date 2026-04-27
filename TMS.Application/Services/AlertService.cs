using System.Diagnostics;
using Microsoft.Extensions.Logging;
using TMS.Application.Interfaces.Services;

namespace TMS.Application.Services;

internal sealed class AlertService(ILogger<AlertService> logger) : IAlertService
{
    private readonly ILogger<AlertService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private static readonly ActivitySource _activitySource = new ActivitySource("TMS.Application");

    public async Task ShowAlertAsync(string title, string message, string cancel)
    {
        using Activity? activity = _activitySource.StartActivity("AlertService.ShowAlertAsync");
        
        _logger.LogInformation("Showing alert with title: {Title}", title);

        await Shell.Current.DisplayAlertAsync(title, message, cancel);

        _logger.LogInformation("Alert displayed successfully");
    }
}
