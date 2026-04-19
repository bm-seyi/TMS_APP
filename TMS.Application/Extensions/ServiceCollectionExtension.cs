using TMS.Core.Interfaces.Services;
using TMS.Core.Services;
using TMS.Core.Interfaces.AuthenticationProviders;
using TMS.Core.AuthenticationProviders;
using TMS.Core.Pipelines.Login;
using TMS.Core.Interfaces.Pipelines;
using TMS.Core.Pipelines;
using TMS.Domain.PipelineContexts;


namespace TMS.Core.Extensions
{
    public static class ServiceCollectionExtension
    {
        extension(IServiceCollection services)
        {
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
                    ));

                return services;
            }

            public IServiceCollection AddAlertService() => services.AddSingleton<IAlertService, AlertService>();
            public IServiceCollection AddNavigationService() => services.AddSingleton<INavigationService, NavigationService>();
        }
    }
}