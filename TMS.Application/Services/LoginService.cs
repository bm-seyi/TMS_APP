using Microsoft.Extensions.Logging;
using System.Diagnostics;
using TMS.Application.Interfaces.Pipelines;
using TMS.Application.Interfaces.Services;
using TMS.Domain.PipelineContexts;


namespace TMS.Application.Services;

internal sealed class LoginService(ILogger<LoginService> logger, IPipelineEngine<LoginContext> pipelineEngine) : ILoginService
{
    private readonly ILogger<LoginService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IPipelineEngine<LoginContext> _pipelineEngine = pipelineEngine ?? throw new ArgumentNullException(nameof(pipelineEngine));
    private static readonly ActivitySource _activitySource = new ActivitySource("TMS.Application");

    public async Task<bool> LoginAsync(LoginContext loginContext, CancellationToken cancellationToken)
    {
        using Activity? activity = _activitySource.StartActivity("LoginService.LoginAsync");

        _logger.LogInformation("Login pipeline started");

        await _pipelineEngine.ExecuteAsync(loginContext, cancellationToken);

        _logger.LogInformation("Login pipeline completed. IsAuthenticated = {IsAuthenticated}", loginContext.IsAuthenticated);

        return loginContext.IsAuthenticated;
    }
}