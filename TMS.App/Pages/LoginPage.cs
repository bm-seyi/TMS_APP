using TMS.App.ViewModels;

namespace TMS.App.Pages
{
	public partial class LoginPage : ContentPage
	{
		public LoginPage(LoginPageViewModel vm)
		{
			InitializeComponent();
			BindingContext = vm ?? throw new ArgumentNullException(nameof(vm));
		}
	}
}