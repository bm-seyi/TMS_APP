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

	private void Unfocused_PasswordMatch(object sender, FocusEventArgs e){
		if (string.IsNullOrEmpty(entry_signupConPassword.Text))
		{
			((Entry)sender).BackgroundColor = Colors.Transparent;
		}
	}

	private async void clicked_signupSubmit(object sender, EventArgs e)
	{
		if (Utils.IsValidEmail(entry_signupEmail.Text) && entry_signupPassword.Text == entry_signupConPassword.Text)
		{
			var data = new Dictionary<string, string>
			{
				{"email", entry_signupEmail.Text}, 
				{"Pwd", entry_signupConPassword.Text},
			};

			if (Connectivity.Current.NetworkAccess != NetworkAccess.None)
			{
				int loginResponse =	await ApiUtilities.PostDataToAPI(data, "Registration/");

				if(loginResponse == 200)
				{
					await Navigation.PushAsync(new Hub());
				} else 
				{
					await DisplayAlert("Incorect Email or Password", "Please try again after a few minutes", "Ok");
				}
			} else 
			{
				await DisplayAlert("Device Isn't Connected to the Internet", "Please  reconnect to the internet", "Ok");
			}


		} else 
		{
			await DisplayAlert("Incorect Email or Password", "Please try again after a few minutes", "Ok");
		}
	}

	private void clicked_loginButton(object sender, EventArgs e)
	{
		signupStack.IsVisible = false;
		loginStack.IsVisible = true;
		loginButton.IsVisible = false;
		signupButton.IsVisible = false;
	}

	private void clicked_signupButton(object sender, EventArgs e)
	{
		signupStack.IsVisible = true;
		loginStack.IsVisible = false;
		loginButton.IsVisible = false;
		signupButton.IsVisible = false;
	}

	private async void clicked_loginSubmit(object sender, EventArgs e)
	{
		if (Utils.IsValidEmail(entry_loginEmail.Text))
		{
			var data = new Dictionary<string, string>
			{
				{"email", entry_loginEmail.Text}, 
				{"Pwd", entry_loginPassword.Text},
			};

			if (Connectivity.Current.NetworkAccess != NetworkAccess.None)
			{
				int loginResponse =	await ApiUtilities.PostDataToAPI(data, "Authentication/");

				if(loginResponse == 200)
				{
					await Navigation.PushAsync(new Hub());
				} else 
				{
					await DisplayAlert("Incorect Email or Password", "Please try again after a few minutes", "Ok");
				}
			} else 
			{
				await DisplayAlert("Device Isn't Connected to the Internet", "Please  reconnect to the internet", "Ok");
			}


		} else 
		{
			await DisplayAlert("Incorect Email or Password", "Please try again after a few minutes", "Ok");
		}
	}
}

