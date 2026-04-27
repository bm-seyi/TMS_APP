using Microsoft.Extensions.Logging;
using System.Diagnostics;
using TMS.Application.Interfaces.Pipelines;
using TMS.Application.Interfaces.Services;
using TMS.Domain.PipelineContexts;


namespace TMS.Application.Pipelines.Login
{
    internal sealed class ArcgisStep(ILogger<ArcgisStep> logger, IArcgisService arcgisService) : IPipelineStep<LoginContext>
    {
        private readonly ILogger<ArcgisStep> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IArcgisService _arcgisService = arcgisService ?? throw new ArgumentNullException(nameof(arcgisService));
        private static readonly ActivitySource _activitySource = new ActivitySource("TMS.Application");

        public async Task InvokeAsync(LoginContext context, Func<Task> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation("ArcgisStep starting.");
            using (Activity? activity = _activitySource.StartActivity("ArcgisStep.InvokeAsync"))
            {
                await _arcgisService.RegisterAsync(cancellationToken);
            }

            _logger.LogInformation("ArcgisStep calling next.");
            await next();
        }
    }
}