using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using Esri.ArcGISRuntime.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using TMS.Core.Extensions;
using TMS.App.Pages;
using TMS.App.ViewModels;
using TMS.App.Controls;
using Microsoft.Maui.Handlers;


namespace TMS.App
{
	[ExcludeFromCodeCoverage]
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			MauiAppBuilder builder = MauiApp.CreateBuilder();

			builder.AddServiceDefaults();

			builder.Configuration
				.AddJsonFile("appsettings.json", optional: false)
				#if DEBUG
				.AddJsonFile("appsettings.Development.json", optional: false)
				#endif
				.AddJsonFile($"appsettings.Production.json", optional: true);
				
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});

			
			EntryHandler.Mapper.AppendToMapping("BorderlessOnly", (handler, view) =>
			{
			#if WINDOWS
				if (view is BorderlessEntry)
				{
					var textBox = handler.PlatformView;
					
					handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
					handler.PlatformView.Background = null;

					textBox.Resources["TextControlBorderBrush"] = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);
					textBox.Resources["TextControlBorderBrushFocused"] = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);
					textBox.Resources["TextControlBorderBrushPointerOver"] = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);
					textBox.Resources["TextControlBorderBrushDisabled"] = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);

					textBox.FocusVisualPrimaryThickness = new Microsoft.UI.Xaml.Thickness(0);
					textBox.FocusVisualSecondaryThickness = new Microsoft.UI.Xaml.Thickness(0);
				}
			#endif
			});
			

			// Other Services
			builder.Services.AddPublicClientApplication(builder.Configuration);
			builder.Services.AddAuthService();
			builder.Services.AddAlertService();
			builder.Services.AddNavigationService();
			builder.Services.AddArcgisService();
			builder.Services.AddTmsClient(builder.Configuration);	
			builder.Services.AddTmsAuthHeaderHandler();		

			builder.Services.AddSingleton(SecureStorage.Default);

			// ViewModels
			builder.Services.AddTransient<LoginPageViewModel>();

			// Views
			builder.Services.AddTransient<AppShell>();
			builder.Services.AddTransient<Hub>();
			builder.Services.AddSingleton<LoginPage>();

			builder.Services.AddLogging(configure =>
			{
				configure.AddConsole();
			});


			builder.UseArcGISRuntime();

			#if DEBUG
				builder.Logging.SetMinimumLevel(LogLevel.Debug);
			#endif

			MauiApp app = builder.Build();
			app.Services.GetRequiredService<AppShell>();

			return app;
		}
	}
}


