using TMS.Infrastructure.Http.DelegatingHandlers;


namespace TMS.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddTmsAuthHeaderHandler() => services.AddTransient<TmsAuthHeaderDelegatingHandler>();
    }
}