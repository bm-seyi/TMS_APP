using Microsoft.Extensions.Logging;

namespace TMS_APP.Pages
{
	public partial class AccessPortal : ContentPage
	{
		private readonly ILogger<AccessPortal> _logger;

		public AccessPortal( ILogger<AccessPortal> logger)
		{
			InitializeComponent();
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		private async void clicked_loginButton(object sender, EventArgs e)
		{
			try
			{
				await Shell.Current.GoToAsync("Hub");
			}
			catch (Exception ex)
			{
				await DisplayAlertAsync("Login Error", ex.Message, "OK");
			}

		}
	}
}