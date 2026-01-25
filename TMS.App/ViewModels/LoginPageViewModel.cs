using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using TMS.Core.Interfaces.Services;


namespace TMS.App.ViewModels
{
    public partial class LoginPageViewModel : ObservableObject
    {
        private readonly ILogger<LoginPageViewModel> _logger;
        private readonly IAuthService _authService;
        private readonly IAlertService _alertService;
        private readonly INavigationService _navigationService;
        private readonly IArcgisService _acrgisService;
        private static readonly ActivitySource _activitySource = new ActivitySource("TMS.App.ViewModels.LoginPageViewModel");

        public LoginPageViewModel(ILogger<LoginPageViewModel> logger, IAuthService authService, IAlertService alertService, INavigationService navigationService, IArcgisService arcgisService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _alertService = alertService ?? throw new ArgumentNullException(nameof(alertService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _acrgisService = arcgisService ?? throw new ArgumentNullException(nameof(arcgisService));
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            using Activity? activity= _activitySource.StartActivity("LoginViewModel.LoginAsync");
            
            _logger.LogInformation("LoginAsync started.");
                
            try
            {
                _logger.LogInformation("Attempting authentication...");

                AuthenticationResult result = await _authService.LoginAsync(CancellationToken.None);

				if (result.AccessToken != null)
				{
                    _logger.LogInformation("Authentication successful. Navigating to Hub.");
                    await _acrgisService.RegisterAsync(CancellationToken.None);
					await _navigationService.NavigateToAsync("Hub");
					return;
				}

                _logger.LogWarning("Authentication failed: AccessToken was null."); 
                await _alertService.ShowAlertAsync("Login Failed", "Authentication result is null.", "OK");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred during LoginAsync.");
                await _alertService.ShowAlertAsync("Login Failed", "Authentication result is null.", "OK");
            }
            finally
            {
                _logger.LogInformation("LoginAsync finished.");
            }
        }
    }
}
