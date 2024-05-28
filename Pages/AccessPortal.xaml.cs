using Microsoft.Maui.Controls;
using TMS_APP.Utilities;

namespace TMS_APP.Pages;

public partial class AccessPortal : ContentPage
{
	public AccessPortal()
	{
		InitializeComponent();
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
			await DisplayAlert("Device Isn't Connected to the Internet", "Please reconnect to the internet", "Ok");
			return;
		} 
		
		int loginResponse =	await ApiUtilities.PostDataToAPI(payload, endpoint);

		if (loginResponse == 200)
		{
			await Navigation.PushAsync(new Hub());
		} 
		else 
		{
			await DisplayAlert($"{action} Failed", "Incorrect Email or Password. Please try again after a few minutes", "Ok");
		}
	}
}

