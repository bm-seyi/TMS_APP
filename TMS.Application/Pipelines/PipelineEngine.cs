using System.Diagnostics;
using TMS.Application.Interfaces.Pipelines;


namespace TMS.Application.Pipelines;

public sealed class PipelineEngine<TContext> : IPipelineEngine<TContext>
{
    private readonly IReadOnlyList<IPipelineStep<TContext>> _steps;
    private static readonly ActivitySource _activitySource = new ActivitySource("TMS.Application");

    public PipelineEngine(IEnumerable<IPipelineStep<TContext>> steps)
    {
        _steps = [.. steps];
    }

    public Task ExecuteAsync(TContext context, CancellationToken cancellationToken)
    {
        using Activity? _ = _activitySource.StartActivity("PipelineEngine.ExecuteAsync");
        
        int index = 0;

        Task Next()
        {
            if (index >= _steps.Count)
                return Task.CompletedTask;

            IPipelineStep<TContext> step = _steps[index++];
            return step.InvokeAsync(context, Next, cancellationToken);
        }

        return Next();
    }
}   

