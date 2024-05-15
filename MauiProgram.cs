using Microsoft.Extensions.Logging;
using DotNetEnv;
using System;
using Esri.ArcGISRuntime.Maui;
using Esri.ArcGISRuntime;

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
