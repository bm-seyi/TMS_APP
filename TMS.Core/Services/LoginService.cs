using Microsoft.Extensions.Logging;
using System.Diagnostics;
using TMS.Core.Interfaces.Pipelines;
using TMS.Core.Interfaces.Services;
using TMS.Models.PipelineContexts;


namespace TMS.Core.Services
{
    internal sealed class LoginService : ILoginService
    {
        private readonly ILogger<LoginService> _logger;
        private readonly IPipelineEngine<LoginContext> _pipelineEngine;
        private static readonly ActivitySource _activitySource = new ActivitySource("TMS.Core.Service.LoginService");

        public LoginService(ILogger<LoginService> logger, IPipelineEngine<LoginContext> pipelineEngine)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _pipelineEngine = pipelineEngine ?? throw new ArgumentNullException(nameof(pipelineEngine));
        }

        public async Task<bool> LoginAsync(LoginContext loginContext, CancellationToken cancellationToken)
        {
            using Activity? activity = _activitySource.StartActivity("LoginService.LoginAsync");

            _logger.LogInformation("Login pipeline started");

            await _pipelineEngine.ExecuteAsync(loginContext, cancellationToken);

            _logger.LogInformation("Login pipeline completed. IsAuthenticated = {IsAuthenticated}", loginContext.IsAuthenticated);

            return loginContext.IsAuthenticated;
        }
    }
}