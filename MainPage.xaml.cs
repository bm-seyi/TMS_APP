using Microsoft.Maui.Controls;
namespace TMS_APP;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private void TextChanged_NumbersOnly(object sender, TextChangedEventArgs e)
	{
		string nextvalue = e.NewTextValue;

		if(!string.IsNullOrEmpty(nextvalue))
		{
			if(!int.TryParse(nextvalue, out _))
			{
				((Entry)sender).Text = e.OldTextValue;
			}
		}
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

	private void Clicked_HandleLoginButton(object sender, EventArgs e)
	{
		int.TryParse(Entry_EmployeeID.Text, out int ID);
		string password = Entry_Password.Text;
		string confirmedpassword = Entry_ConfirmedPassword.Text;
	}
}

