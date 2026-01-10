using TMS.App.Pages;
namespace TMS.App;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute("LoginPage", typeof(LoginPage));
		Routing.RegisterRoute("Hub", typeof(Hub));
	}
}
