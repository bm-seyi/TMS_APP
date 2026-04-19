using Microsoft.Extensions.Logging;
using System.Diagnostics;
using TMS.Application.Interfaces.Pipelines;
using TMS.Application.Interfaces.Services;
using TMS.Domain.PipelineContexts;


namespace TMS.Application.Pipelines.Login
{
    internal sealed class ArcgisStep : IPipelineStep<LoginContext>
    {
        private readonly ILogger<ArcgisStep> _logger;
        private readonly IArcgisService _arcgisService;
        private static readonly ActivitySource _activitySource = new ActivitySource("TMS.Application");

        public ArcgisStep(ILogger<ArcgisStep> logger, IArcgisService arcgisService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _arcgisService = arcgisService ?? throw new ArgumentNullException(nameof(arcgisService));
        }

        public async Task InvokeAsync(LoginContext context, Func<Task> next, CancellationToken cancellationToken)
        {
            using (Activity? activity = _activitySource.StartActivity("ArcgisStep.InvokeAsync"))
            {
                await _arcgisService.RegisterAsync(cancellationToken);
            }

            await next();
        }
    }
}