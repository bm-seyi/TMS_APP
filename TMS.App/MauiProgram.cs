using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using Esri.ArcGISRuntime.Maui;
using Esri.ArcGISRuntime;
using IBrowser = Duende.IdentityModel.OidcClient.Browser.IBrowser;
using DotNetEnv;
using TMS_APP.Pages;
using TMS_APP.AccessControl;
using TMS_APP.HubServices;
using TMS_APP.MauiProgramExtension;
using TMS_APP.OIDC;
using Microsoft.Extensions.Configuration;


namespace TMS_APP
{
	[ExcludeFromCodeCoverage]
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			MauiAppBuilder builder = MauiApp.CreateBuilder();

			builder.Configuration
				.AddJsonFile("appsettings.json", optional: false)
				.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);
				
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});

			builder.Services.AddTransient<AppShell>();
			builder.Services.AddTransient<Hub>();
			builder.Services.AddTransient<IBrowser, BrowserService>();
			builder.Services.AddTransient<IAuthService, AuthService>();
			builder.Services.AddTransient<IAuthClient, AuthClient>();


			builder.Services.AddSingleton<AccessPortal>();
			builder.Services.AddSingleton<ILinesHubService, LinesHubService>();
			builder.Services.AddSingleton<ISecureStorage>(SecureStorage.Default);

			builder.Services.AddOidcAuthentication(options =>
			{
				options.Authority = "https://localhost:8443/realms/maui_realm";
				options.ClientId = "maui_client";
				options.Scope = "signalR.read offline_access";
				options.RedirectUri = "tmsapp://callback/";
				options.PostLogoutRedirectUri = "tmsapp://logout-callback/";
				options.DisablePushedAuthorization = false;
			});


			builder.Services.AddLogging(configure =>
			{
				configure.AddConsole();
			});

			string mapKey = Environment.GetEnvironmentVariable("arcgisKey") ?? throw new ArgumentNullException(nameof(mapKey));

			builder.UseArcGISRuntime(config =>
			{
				config.UseApiKey(mapKey);
			});


			#if DEBUG
				builder.Logging.SetMinimumLevel(LogLevel.Debug);
			#endif

			MauiApp app = builder.Build();
			app.Services.GetRequiredService<AppShell>();

			return app;
		}
	}
}


