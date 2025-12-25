using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using Windows.ApplicationModel.Activation;
using System.Web;
using CommunityToolkit.Mvvm.Messaging;
using TMS_APP.InternalMessages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TMS_APP.WinUI;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : MauiWinUIApplication
{
	/// <summary>
	/// Initializes the singleton application object.  This is the first line of authored code
	/// executed, and as such is the logical equivalent of main() or WinMain().
	/// </summary>
	public App()
	{
		this.InitializeComponent();
	}

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

	protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
	{
		base.OnLaunched(args);

		var actEventArgs = AppInstance.GetCurrent().GetActivatedEventArgs();
		if (actEventArgs.Kind == ExtendedActivationKind.Protocol)
		{
			IProtocolActivatedEventArgs? protocolActivatedEventArgs = actEventArgs.Data as IProtocolActivatedEventArgs;
			if (protocolActivatedEventArgs != null)
			{
				HandleProtocolUri(protocolActivatedEventArgs.Uri.AbsoluteUri);
			}
		}
	}

	private void HandleProtocolUri(string uri)
	{
		// Example: Parse the URI to extract parameters
		if (uri.StartsWith("tmsapp://auth"))
		{
			WeakReferenceMessenger.Default.Send(new ProtocolUriMessage(uri));
		}
	}
}

