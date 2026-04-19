using Microsoft.Extensions.Configuration;
using TMS.Application.Interfaces.HttpClients;
using TMS.Core.Interfaces.Services;
using TMS.Infrastructure.Http.Clients;
using TMS.Infrastructure.Http.DelegatingHandlers;
using TMS.Infrastructure.Services;
using Microsoft.Identity.Client;
using Microsoft.Extensions.Options;
using TMS.Domain.Configuration;


namespace TMS.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddPublicClientApplication()
        {
            return services.AddSingleton(sp =>
            {
                AzureAdOptions azureAdOptions = sp.GetRequiredService<IOptions<AzureAdOptions>>().Value;
                
                return PublicClientApplicationBuilder.Create(azureAdOptions.ClientId)
                .WithRedirectUri(azureAdOptions.RedirectUri)
                .Build();
            });
        }

        public IServiceCollection AddTmsAuthHeaderHandler() => services.AddTransient<TmsAuthHeaderDelegatingHandler>();
        public IServiceCollection AddTmsClient(IConfiguration configuration)
        {
            services.AddHttpClient<ITmsClient, TmsClient>(x =>
            {
                string url = configuration["TMS:Url"] ?? throw new InvalidOperationException("Unable to retrieve tms url from configuration");
                x.BaseAddress = new Uri(url);
            })
            .AddHttpMessageHandler<TmsAuthHeaderDelegatingHandler>();

            return services;
        }

        public IServiceCollection AddArcgisService() => services.AddTransient<IArcgisService, ArcgisService>();

        public IServiceCollection AddMicrosoftAuthService() => services.AddTransient<IMicrosoftAuthService, MicrosoftAuthService>();
    }
}