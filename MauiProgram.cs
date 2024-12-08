using Microsoft.Extensions.Logging;
using DotNetEnv;
using Esri.ArcGISRuntime.Maui;
using Esri.ArcGISRuntime;
using TMS_APP.Utilities;
using TMS_APP.Pages;
using IBrowser = IdentityModel.OidcClient.Browser.IBrowser;

namespace TMS_APP;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		Env.Load(Path.Combine(Environment.CurrentDirectory, "Resources/EnvironmentVariable/.env"));

		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		
		// Dependency Injection - OTB
		builder.Services.AddHttpClient();
		builder.Services.AddLogging(configure => 
		{
			configure.AddConsole();
			configure.AddDebug();
		});

		// Dependency Injection - Transient
		builder.Services.AddTransient<AppShell>();
		builder.Services.AddTransient<Hub>();
		builder.Services.AddTransient<IApiUtilities, ApiUtilities>();
		builder.Services.AddTransient<IBrowser, BrowserService>();


		// Dependency Injection - Singleton
		builder.Services.AddSingleton<AccessPortal>();
		builder.Services.AddSingleton<IAuthService, AuthService>();
		
		string? mapKey = Environment.GetEnvironmentVariable("arcgisKey");
		if (!string.IsNullOrWhiteSpace(mapKey))
		{
			builder.UseArcGISRuntime(config => 
			{
				config.UseApiKey(mapKey);
			});
		}
		

#if DEBUG
		builder.Logging.SetMinimumLevel(LogLevel.Debug);
		builder.Logging.AddDebug();
#endif

		var app =  builder.Build();
		app.Services.GetRequiredService<AppShell>();

		
		return app;
	}
}
