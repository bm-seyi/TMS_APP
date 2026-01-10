using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using TMS.Core.Interfaces.Services;

namespace TMS_APP.Pages
{
	public partial class AccessPortal : ContentPage
	{
		private readonly ILogger<AccessPortal> _logger;
		private readonly IAuthService _authService;

		public AccessPortal( ILogger<AccessPortal> logger, IAuthService authService)
		{
			InitializeComponent();
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_authService = authService ?? throw new ArgumentNullException(nameof(authService));
		}

		private async void ClickedLoginButton(object sender, EventArgs e)
		{
			try
			{
				AuthenticationResult result = await _authService.LoginAsync(CancellationToken.None);

				if (result.AccessToken != null)
				{
					await Shell.Current.GoToAsync("Hub");
					return;
				}
		
				await DisplayAlertAsync("Login Failed", "Authentication result is null.", "OK");
			}
			catch (Exception ex)
			{
				await DisplayAlertAsync("Login Error", ex.Message, "OK");
			}

		}
	}
}