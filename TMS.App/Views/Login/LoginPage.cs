using TMS.App.ViewModels;

namespace TMS.App.Views.Login;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel loginViewModel)
	{
		InitializeComponent();
		BindingContext = loginViewModel ?? throw new ArgumentNullException(nameof(loginViewModel));
	}
}
