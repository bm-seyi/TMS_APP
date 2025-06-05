using Microsoft.Extensions.Logging;
using TMS_APP.AccessControl;

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

		private async void clicked_loginButton(object sender, EventArgs e)
		{
			try
			{
				await _authService.LoginAsync();
				await Shell.Current.GoToAsync("Hub");
			}
			catch (Exception ex)
			{
				await DisplayAlert("Login Error", ex.Message, "OK");
			}

		}
	}
}