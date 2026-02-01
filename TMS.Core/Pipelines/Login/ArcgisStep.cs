using Microsoft.Extensions.Logging;
using System.Diagnostics;
using TMS.Core.Interfaces.Pipeline;
using TMS.Core.Interfaces.Services;
using TMS.Models.PipelineContexts;


namespace TMS.Core.Pipelines.Login
{
    internal sealed class ArcgisStep : IPipelineStep<LoginContext>
    {
        private readonly ILogger<ArcgisStep> _logger;
        private readonly IArcgisService _arcgisService;
        private static readonly ActivitySource _activitySource = new ActivitySource("TMS.Core.Pipelines.Login.ArcgisStep");

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