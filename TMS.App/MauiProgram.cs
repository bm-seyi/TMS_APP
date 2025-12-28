using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using Esri.ArcGISRuntime.Maui;
using Esri.ArcGISRuntime;
using TMS_APP.Pages;
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


			builder.Services.AddSingleton<AccessPortal>();
			builder.Services.AddSingleton(SecureStorage.Default);


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


