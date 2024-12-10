using TMS_APP.Pages;

namespace TMS_APP;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute("AccessPortal", typeof(AccessPortal));
		Routing.RegisterRoute("Hub", typeof(Hub));
	}
}
