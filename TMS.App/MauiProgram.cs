using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using Esri.ArcGISRuntime.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using TMS.Core.Extensions;
using TMS.App.Pages;
using TMS.App.ViewModels;


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


