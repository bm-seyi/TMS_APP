using System.Diagnostics;
using Microsoft.Extensions.Logging;
using TMS.Core.Interfaces.Services;

namespace TMS.Core.Services
{
    internal sealed class AlertService : IAlertService
    {
        private readonly ILogger<AlertService> _logger;
        private static readonly ActivitySource _activitySource = new ActivitySource("TMS.Core.Services.AlertService");

        public AlertService(ILogger<AlertService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ShowAlertAsync(string title, string message, string cancel)
        {
            using Activity? activity = _activitySource.StartActivity("AlertService.ShowAlertAsync");
            
            _logger.LogInformation("Showing alert with title: {Title}", title);

            await Shell.Current.DisplayAlertAsync(title, message, cancel);

            _logger.LogInformation("Alert displayed successfully");
        }
    }
}