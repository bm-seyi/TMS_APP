using Microsoft.Extensions.Logging;
using System.Collections.Frozen;
using System.Diagnostics;
using TMS.Application.Interfaces.AuthenticationProviders;
using TMS.Application.Interfaces.Pipelines;
using TMS.Domain;
using TMS.Domain.PipelineContexts;


namespace TMS.Application.Pipelines.Login;

internal sealed class LoginStep(ILogger<LoginStep> logger, IEnumerable<IAuthenticationProvider> providers) : IPipelineStep<LoginContext>
{
    private readonly ILogger<LoginStep> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly FrozenDictionary<AuthenticationProvider, IAuthenticationProvider> _providers = providers.ToFrozenDictionary(x => x.AuthenticationProvider) ?? throw new ArgumentNullException(nameof(providers));
    private static readonly ActivitySource _activitySource = new ActivitySource("TMS.Application");

    public async Task InvokeAsync(LoginContext context, Func<Task> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("LoginStep starting.");
        using (Activity? activity = _activitySource.StartActivity("LoginStep.InvokeAsync"))
        {
            IAuthenticationProvider strategy = _providers[context.AuthenticationProvider];

            await strategy.AuthenticateAsync(context, cancellationToken);
        }
        _logger.LogInformation("LoginStep calling next.");
        await next();
    }
}
