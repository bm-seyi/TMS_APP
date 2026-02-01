using Microsoft.Extensions.Logging;
using System.Collections.Frozen;
using System.Diagnostics;
using TMS.Core.Interfaces.AuthenticationProviders;
using TMS.Core.Interfaces.Pipeline;
using TMS.Models;
using TMS.Models.PipelineContexts;


namespace TMS.Core.Pipelines.Login
{
    internal sealed class LoginStep : IPipelineStep<LoginContext>
    {
        private readonly ILogger<LoginStep> _logger;
        private readonly FrozenDictionary<AuthenticationProvider, IAuthenticationProvider> _providers;
        private static readonly ActivitySource _activitySource = new ActivitySource("TMS.Core.Pipelines.LoginStep");

        public LoginStep(ILogger<LoginStep> logger, IEnumerable<IAuthenticationProvider>  providers)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _providers = providers.ToFrozenDictionary(x => x.AuthenticationProvider) ?? throw new ArgumentNullException(nameof(providers));
        }

        public async Task InvokeAsync(LoginContext context, Func<Task> next, CancellationToken cancellationToken)
        {
            using (Activity? activity = _activitySource.StartActivity("LoginStep.InvokeAsync"))
            {
                IAuthenticationProvider strategy = _providers[context.AuthenticationProvider];

                await strategy.AuthenticateAsync(context, cancellationToken);
            }

            await next();
        }
    }
}