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

		if (IsValidEmailAddress(Entry_Email.Text) && Entry_Password.Text == Entry_ConfirmedPassword.Text)
		{
			PostRegistration();
		}
	}

	private bool IsValidEmailAddress(string email)
	{
		Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
		return regex.IsMatch(email);
	}

	private async void PostRegistration()
	{
		using (HttpClient client = new HttpClient()) 
		{
			string endpoint = "http://localhost:5188/";

			var data = new Dictionary<string, string>
			{
				{"email", Entry_Email.Text}, 
				{"Pwd", Entry_ConfirmedPassword.Text},
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

