using TMS.App.Views.Hub;
using TMS.App.Views.Login;

namespace TMS.App;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute("LoginPage", typeof(LoginPage));
		Routing.RegisterRoute("Hub", typeof(HubPage));
	}
}
