using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;
using Esri.ArcGISRuntime.Maui;
using Esri.ArcGISRuntime;
using IBrowser = IdentityModel.OidcClient.Browser.IBrowser;
using DotNetEnv;
using TMS_APP.Utilities;
using TMS_APP.Pages;
using Polly;

namespace TMS_APP
{
	[ExcludeFromCodeCoverage]
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			#if DEBUG
				string parentDirectory = Directory.GetParent(Environment.CurrentDirectory)?.FullName ?? throw new ArgumentNullException(nameof(parentDirectory));
				Env.Load(Path.Combine(parentDirectory, ".env"));
			#endif

			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});

			builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
			{
				client.BaseAddress = new Uri("https://localhost:5188/");
			});

			builder.Services.AddTransient<AppShell>();
			builder.Services.AddTransient<Hub>();
			builder.Services.AddTransient<IBrowser, BrowserService>();


			builder.Services.AddSingleton<AccessPortal>();


			builder.Services.AddLogging(configure =>
			{
				configure.AddConsole();
				configure.AddDebug();
			});

			string mapKey = Environment.GetEnvironmentVariable("arcgisKey") ?? throw new ArgumentNullException(nameof(mapKey));
			if (string.IsNullOrWhiteSpace(mapKey))
			{
				throw new ArgumentNullException("ArcGIS API Key is not set in the environment variables.");
			}

			builder.UseArcGISRuntime(config =>
			{
				config.UseApiKey(mapKey);
			});



#if DEBUG
			builder.Logging.SetMinimumLevel(LogLevel.Debug);
			builder.Logging.AddDebug();
#endif

			var app = builder.Build();
			app.Services.GetRequiredService<AppShell>();


			return app;
		}
	}
}


