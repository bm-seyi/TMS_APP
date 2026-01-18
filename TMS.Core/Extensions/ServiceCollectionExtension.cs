using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using TMS.Core.HttpClients;
using TMS.Core.Interfaces.HttpClients;
using TMS.Core.Interfaces.Services;
using TMS.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using TMS.Core.Handlers;

namespace TMS.Core.Extensions
{
    public static class ServiceCollectionExtension
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection AddPublicClientApplication(IConfiguration configuration)
            {
                return services.AddSingleton(sp =>
                {
                    string clientId = configuration.GetRequiredValue<string>("AzureAD:ClientId");
                    
                    return PublicClientApplicationBuilder.Create(clientId)
                    .WithRedirectUri(configuration.GetRequiredValue<string>("AzureAD:RedirectUri"))
                    .Build();
                });
            }

            public IServiceCollection AddAuthService() => services.AddTransient<IAuthService, AuthService>();
            public IServiceCollection AddAlertService() => services.AddSingleton<IAlertService, AlertService>();
            public IServiceCollection AddNavigationService() => services.AddSingleton<INavigationService, NavigationService>();
            public IServiceCollection AddArcgisService() => services.AddTransient<IArcgisService, ArcgisService>();
            public IServiceCollection AddTmsAuthHeaderHandler() => services.AddTransient<TmsAuthHeaderHandler>();
            public IServiceCollection AddTmsClient(IConfiguration configuration)
            {
                services.AddHttpClient<ITmsClient, TmsClient>(x =>
                {
                    x.BaseAddress = new Uri(configuration.GetRequiredValue<string>("TMS:Url"));
                })
                .AddHttpMessageHandler<TmsAuthHeaderHandler>();

                return services;
            }
        }
    }
}