using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using TMS.Core.HttpClients;
using TMS.Core.Interfaces.HttpClients;
using TMS.Core.Interfaces.Services;
using TMS.Core.Services;
using TMS.Core.Handlers;
using TMS.Core.Interfaces.AuthenticationProviders;
using TMS.Core.AuthenticationProviders;
using TMS.Models.PipelineContexts;
using TMS.Core.Pipelines.Login;
using TMS.Core.Interfaces.Pipelines;
using TMS.Core.Pipelines;


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

            public IServiceCollection AddMicrosoftAuthService() => services.AddTransient<IMicrosoftAuthService, MicrosoftAuthService>();
            public IServiceCollection AddLoginService() => services.AddTransient<ILoginService, LoginService>();
            public IServiceCollection AddLoginPipeline()
            {
                // Providers
                services.AddTransient<IAuthenticationProvider, MicrosoftAuthProvider>();

                // Steps
                services.AddTransient<IPipelineStep<LoginContext>, LoginStep>();
                services.AddTransient<IPipelineStep<LoginContext>, ArcgisStep>();

                //Pipeline
               services.AddTransient<IPipelineEngine<LoginContext>>(sp =>
                new PipelineEngine<LoginContext>(
                    sp.GetServices<IPipelineStep<LoginContext>>()
                )
);

                return services;
            }

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