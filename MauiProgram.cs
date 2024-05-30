using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using DotNetEnv;
using System;
using Esri.ArcGISRuntime.Maui;
using Esri.ArcGISRuntime;
using TMS_APP.Utilities;
using TMS_APP.Pages;

namespace TMS_APP;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		Env.Load(Path.Combine(Environment.CurrentDirectory, "TMS_APP/Resources/EnvironmentVariable/.env"));
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		
		builder.Services.AddHttpClient();
		builder.Services.AddSingleton<ApiUtilities>();
		builder.Services.AddSingleton<AccessPortal>();
		builder.Services.AddLogging(configure => configure.AddConsole());
		
		string? mapKey = Environment.GetEnvironmentVariable("arcgisKey");
		if (!string.IsNullOrWhiteSpace(mapKey))
		{
			builder.UseArcGISRuntime(config => 
			{
				config.UseApiKey(mapKey);
			});
		}
		

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
