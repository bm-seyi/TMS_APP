using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using TMS.Core.Interfaces.Services;
using TMS.Core.Services;

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
        }
    }
}