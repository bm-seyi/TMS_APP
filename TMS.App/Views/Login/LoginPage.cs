using TMS.App.ViewModels;

namespace TMS.App.Views.Login;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm ?? throw new ArgumentNullException(nameof(vm));
	}
}
