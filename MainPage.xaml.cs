using Microsoft.Maui.Controls;
using System.Text.RegularExpressions;

namespace TMS_APP;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private void TextChanged_PasswordMatch(object sender, TextChangedEventArgs e)
	{
		((Entry)sender).BackgroundColor = (Entry_Password.Text != e.NewTextValue) ? Color.FromRgb(255, 0, 0) : Color.FromRgb(255, 255, 255);
	}

	private void Unfocused_PasswordMatch(object sender, FocusEventArgs e){
		if (string.IsNullOrEmpty(Entry_ConfirmedPassword.Text))
		{
			((Entry)sender).BackgroundColor = Color.FromRgb(255, 255, 255);
		}
	}

	private void Clicked_SignUpButton(object sender, EventArgs e)
	{
		string password = Entry_Password.Text;
		string confirmedpassword = Entry_ConfirmedPassword.Text;

		if (!IsValidEmailAddress(Entry_Email.Text))
		{
			DisplayAlert("Error", "Please enter a valid email address", "OK");
		}
	}

	private bool IsValidEmailAddress(string email)
	{
		Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
		return regex.IsMatch(email);
	}
}

