using Microsoft.Extensions.Logging;
using DotNetEnv;
using System;
using CommunityToolkit.Maui.Maps;
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
		
		builder.UseMauiCommunityToolkitMaps("");

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
