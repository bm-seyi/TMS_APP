using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using TMS_APP.Models;
using TMS_APP.Utilities;

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

		private void TextChanged_PasswordMatch(object sender, TextChangedEventArgs e)
		{
			((Entry)sender).BackgroundColor = (entry_signupPassword.Text != e.NewTextValue) ? Color.FromRgb(255, 0, 0) : Colors.Transparent;
		}

		private void Unfocused_PasswordMatch(object sender, FocusEventArgs e)
		{
			var entry = (Entry)sender;
			entry.BackgroundColor = string.IsNullOrEmpty(entry_signupConPassword.Text) ? Colors.Transparent : entry.BackgroundColor;
		}

		private async void clicked_signupSubmit(object sender, EventArgs e)
		{
			Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
			if (!regex.IsMatch(entry_signupEmail.Text) && entry_signupPassword.Text != entry_signupConPassword.Text)
			{
				_logger.LogWarning("Incorrect Email or Password has been provided");
				await DisplayAlert("Incorrect Email or Password", "Please try again after a few minutes", "Ok");
				return;
			}

			// TODO: Implement the registration logic here
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
				_logger.LogWarning(ex.Message);
				if (ex.InnerException != null)
				{
					_logger.LogWarning(ex.InnerException.Message);
				}
				await DisplayAlert("Error", ex.Message, "OK");
			}
		}

		private void clicked_signupButton(object sender, EventArgs e)
		{
			ToggleStack(true);
		}

		private void ToggleStack(bool isSignup)
		{
			signupStack.IsVisible = isSignup;
			loginButton.IsVisible = false;
			signupButton.IsVisible = false;
		}
	}
}