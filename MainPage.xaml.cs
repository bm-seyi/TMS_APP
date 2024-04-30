using Microsoft.Maui.Controls;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Headers;

namespace TMS_APP;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private void TextChanged_PasswordMatch(object sender, TextChangedEventArgs e)
	{
		((Entry)sender).BackgroundColor = (Entry_signupPassword.Text != e.NewTextValue) ? Color.FromRgb(255, 0, 0) : Colors.Transparent;
	}

	private void Unfocused_PasswordMatch(object sender, FocusEventArgs e){
		if (string.IsNullOrEmpty(Entry_ConfirmedPassword.Text))
		{
			((Entry)sender).BackgroundColor = Colors.Transparent;
		}
	}

	private void clicked_signupSubmit(object sender, EventArgs e)
	{
		string password = Entry_signupPassword.Text;
		string confirmedpassword = Entry_ConfirmedPassword.Text;

		if (IsValidEmailAddress(Entry_signupEmail.Text) && Entry_signupPassword.Text == Entry_ConfirmedPassword.Text)
		{
			PostRegistration(Entry_signupEmail.Text, Entry_ConfirmedPassword.Text);
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

	private void clicked_loginSubmit(object sender, EventArgs e)
	{
		if (IsValidEmailAddress(entry_loginEmail.Text))
		{
			PostRegistration(entry_loginEmail.Text, entry_loginPassword.Text);
		}
	}

	private bool IsValidEmailAddress(string email)
	{
		Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
		return regex.IsMatch(email);
	}

	private async void PostRegistration(string email, string password)
	{
		using (HttpClient client = new HttpClient()) 
		{
			string endpoint = "http://localhost:5188/";

			var data = new Dictionary<string, string>
			{
				{"email", email}, 
				{"Pwd", password},
			};

			var JSON_data = JsonSerializer.Serialize(data);
			client.BaseAddress = new Uri(endpoint);
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

			var content = new StringContent(JSON_data, System.Text.Encoding.UTF8, "application/json");

			var post_registration = await client.PostAsync("Registration/", content);

			if (post_registration.IsSuccessStatusCode)
			{
				var response = await post_registration.Content.ReadAsStringAsync();
				Console.WriteLine(response);
			} else 
			{
				Console.WriteLine(post_registration.StatusCode);
			}

		}
	}
}

