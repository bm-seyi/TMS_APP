using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using DotNetEnv;
using System;
using Esri.ArcGISRuntime.Maui;
using Esri.ArcGISRuntime;
using TMS_APP.Utilities.API;
using TMS_APP.Pages;


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
		
		builder.Services.AddHttpClient();
		builder.Services.AddTransient<IApiUtilities, ApiUtilities>();
		builder.Services.AddSingleton<AccessPortal>();
		builder.Services.AddTransient<Hub>();
		builder.Services.AddLogging(configure => configure.AddConsole());
		
		string? mapKey = Environment.GetEnvironmentVariable("arcgisKey");
		if (!string.IsNullOrWhiteSpace(mapKey))
		{
			builder.UseArcGISRuntime(config => 
			{
				config.UseApiKey(mapKey);
			});
		}
		

#if DEBU
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
