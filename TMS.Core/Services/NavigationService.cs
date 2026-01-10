using Microsoft.Extensions.Logging;
using TMS.Core.Interfaces.Services;

namespace TMS.Core.Services
{
    internal sealed class NavigationService : INavigationService
    {
        private readonly ILogger<NavigationService> _logger;

        public NavigationService(ILogger<NavigationService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task NavigateToAsync(string route)
        {
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
            _logger.LogInformation("Navigating back");
            return Shell.Current?.GoToAsync("..") ?? Task.CompletedTask;
        }
    }
}