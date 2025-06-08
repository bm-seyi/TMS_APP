using Duende.IdentityModel.OidcClient;
using TMS_APP.OIDC;

namespace TMS_APP.MauiProgramExtension
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddOidcAuthentication(this IServiceCollection services, Action<OidcClientOptions> configureOptions)
        {
       
            services.AddTransient<IAuthClient>(sp =>
            {
                OidcClientOptions options = new OidcClientOptions();
                configureOptions(options);

                OidcClient client = new OidcClient(options);
                return new AuthClient(client);
            });

            return services;
        }
    }
}
