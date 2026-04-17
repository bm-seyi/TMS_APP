using Microsoft.Extensions.Configuration;
using TMS.Application.Interfaces.HttpClients;
using TMS.Core.Interfaces.Services;
using TMS.Infrastructure.Http.Clients;
using TMS.Infrastructure.Http.DelegatingHandlers;
using TMS.Infrastructure.Services;


namespace TMS.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    extension(IServiceCollection services)
    {
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

        public IServiceCollection AddMicrosoftAuthService() => services.AddTransient<IMicrosoftAuthService, MicrosoftAuthService>();

    }
}