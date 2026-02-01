using Microsoft.Extensions.Logging;
using System.Diagnostics;
using TMS.Core.Interfaces.Pipeline;
using TMS.Core.Interfaces.Services;
using TMS.Models.PipelineContexts;


namespace TMS.Core.Services
{
    internal sealed class LoginService : ILoginService
    {
        private readonly ILogger<LoginService> _logger;
        private readonly IPipeline<LoginContext> _pipeline;
        private static readonly ActivitySource _activitySource = new ActivitySource("TMS.Core.Service.LoginService");

        public LoginService(ILogger<LoginService> logger, IPipeline<LoginContext> pipeline)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
        }

        public async Task<bool> LoginAsync(LoginContext loginContext, CancellationToken cancellationToken)
        {
            using Activity? activity = _activitySource.StartActivity("LoginService.LoginAsync");

            await _pipeline.ExecuteAsync(loginContext, cancellationToken);

            return loginContext.IsAuthenticated;
        }
    }
}