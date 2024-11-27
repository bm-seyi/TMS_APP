using Microsoft.Extensions.Logging;
using TMS_APP.Utilities;

namespace TMS_APP.Pages;

public partial class AccessPortal : ContentPage
{
	private readonly IApiUtilities _apiUtilities;
	private readonly ILogger<AccessPortal> _logger;
	private readonly IServiceProvider _serviceProvider;

    public AccessPortal(IApiUtilities apiUtilities, ILogger<AccessPortal> logger, IServiceProvider serviceProvider)
    {
        InitializeComponent();
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _apiUtilities = apiUtilities ?? throw new ArgumentNullException(nameof(apiUtilities));
		_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
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
		if (!Utils.IsValidEmail(entry_signupEmail.Text) && entry_signupPassword.Text != entry_signupConPassword.Text)
		{
			_logger.LogWarning("Incorrect Email or Password has been provided");
			await DisplayAlert("Incorrect Email or Password", "Please try again after a few minutes", "Ok");
			return;
		} 
		
		var data = new Dictionary<string, string>
		{
			{"email", entry_signupEmail.Text}, 
			{"Pwd", entry_signupConPassword.Text},
		};

		await ProcessRequest(payload: data, endpoint: "Registration/", action: "Signup");
	}

	private void clicked_loginButton(object sender, EventArgs e)
	{
		ToggleStack(false);
	}

	private void clicked_signupButton(object sender, EventArgs e)
	{
		ToggleStack(true);
	}

	private async void clicked_loginSubmit(object sender, EventArgs e)
	{
		if (!Utils.IsValidEmail(entry_loginEmail.Text))
		{
			_logger.LogWarning("Incorrect Email or Password has been provided");
			await DisplayAlert("Incorrect Email or Password", "Please try again after a few minutes", "Ok"); 
			return;
		}

		var data = new Dictionary<string, string>
		{
			{"email", entry_loginEmail.Text}, 
			{"Pwd", entry_loginPassword.Text},
		};
		
		await ProcessRequest(payload: data, endpoint: "Authentication/", action: "Login");
	}
	private void ToggleStack(bool isSignup)
	{
		signupStack.IsVisible = isSignup;
		loginStack.IsVisible = !isSignup;
		loginButton.IsVisible = false;
		signupButton.IsVisible = false;
	}

	private async Task ProcessRequest(Dictionary<string, string> payload, string endpoint, string action)
	{
		if (Connectivity.Current.NetworkAccess == NetworkAccess.None)
		{
			_logger.LogWarning("Internet Connectivity Lost");
			await DisplayAlert("Device Isn't Connected to the Internet", "Please reconnect to the internet", "Ok");
			return;
		} 
		
		int loginResponse =	await _apiUtilities.PostDataToAPI(payload, endpoint);

		if (loginResponse == 200)
		{
			_logger.LogInformation("User has been authenticated");
			Hub hubPage = _serviceProvider.GetRequiredService<Hub>();
			await Navigation.PushAsync(hubPage);
		} 
		else 
		{
			await DisplayAlert($"{action} Failed", "Incorrect Email or Password. Please try again after a few minutes", "Ok");
		}
	}
}

