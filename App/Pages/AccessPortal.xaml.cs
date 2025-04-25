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

		private void clicked_signupButton(object sender, EventArgs e)
		{
			// TODO: Implement the registration logic here
		}

		private void clicked_loginButton(object sender, EventArgs e)
		{
			// TODO: Implement the login logic here
		}

	
	}
}