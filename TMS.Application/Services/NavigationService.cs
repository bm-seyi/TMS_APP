using System.Diagnostics;
using Microsoft.Extensions.Logging;
using TMS.Application.Interfaces.Services;

namespace TMS.Application.Services;

internal sealed class NavigationService(ILogger<NavigationService> logger) : INavigationService
{
    private readonly ILogger<NavigationService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private static readonly ActivitySource _activitySource = new ActivitySource("TMS.Application");

    public Task NavigateToAsync(string route)
    {
        using Activity? _ = _activitySource.StartActivity("NavigationService.NavigateToAsync");

        if (string.IsNullOrWhiteSpace(route))
            throw new ArgumentNullException(nameof(route));

        _logger.LogInformation("Navigating to route: {Route}", route);

        if (Shell.Current is null)
        {
            _logger.LogWarning("Navigation failed: Shell.Current is null");
            return Task.CompletedTask;
        }
            
        return Shell.Current.GoToAsync(route);
    }

    public Task GoBackAsync()
    {
        using Activity? _ = _activitySource.StartActivity("NavigationService.GoBackAsync");

        _logger.LogInformation("Navigating back");
        return Shell.Current?.GoToAsync("..") ?? Task.CompletedTask;
    }
}