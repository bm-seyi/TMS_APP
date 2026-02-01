using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using TMS.Core.Interfaces.Services;
using TMS.Models;
using TMS.Models.PipelineContexts;


namespace TMS.App.ViewModels
{
    public partial class LoginPageViewModel : ObservableObject
    {
        private readonly ILogger<LoginPageViewModel> _logger;
        private readonly ILoginService _loginService;
        private readonly IAlertService _alertService;
        private readonly INavigationService _navigationService;
        private static readonly ActivitySource _activitySource = new ActivitySource("TMS.App.ViewModels.LoginPageViewModel");

        public LoginPageViewModel(ILogger<LoginPageViewModel> logger, ILoginService loginService, IAlertService alertService, INavigationService navigationService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
            _alertService = alertService ?? throw new ArgumentNullException(nameof(alertService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        [RelayCommand]
        private async Task MicrosoftLoginAsync()
        {
            using Activity? activity= _activitySource.StartActivity("LoginViewModel.LoginAsync");
            
            _logger.LogInformation("LoginAsync started.");
                
            try
            {
                _logger.LogInformation("Attempting authentication...");

                LoginContext loginContext = new LoginContext()
                {
                    AuthenticationProvider = AuthenticationProvider.Microsoft
                };

                bool loginSucceeded = await _loginService.LoginAsync(loginContext, CancellationToken.None);

				if (loginSucceeded)
				{
                    _logger.LogInformation("Authentication successful. Navigating to Hub.");
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
