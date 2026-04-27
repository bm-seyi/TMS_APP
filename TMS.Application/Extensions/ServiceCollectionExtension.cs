using TMS.Application.Interfaces.Services;
using TMS.Application.Interfaces.AuthenticationProviders;
using TMS.Application.Interfaces.Pipelines;
using TMS.Domain.PipelineContexts;
using TMS.Application.AuthenticationProviders;
using TMS.Application.Pipelines;
using TMS.Application.Services;
using TMS.Application.Pipelines.Login;


namespace TMS.Application.Extensions;

public static class ServiceCollectionExtension
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddLoginService() => services.AddTransient<ILoginService, LoginService>();
        public IServiceCollection AddLoginPipeline()
        {
            // Providers
            services.AddTransient<IAuthenticationProvider, MicrosoftAuthenticationProvider>();

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
