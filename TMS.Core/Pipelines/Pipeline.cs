using TMS.Core.Interfaces.Pipeline;

namespace TMS.Core.Pipelines
{
    public class Pipeline<TContext> : IPipeline<TContext>
    {
        private readonly IReadOnlyList<IPipelineStep<TContext>> _steps;

        public Pipeline(IEnumerable<IPipelineStep<TContext>> steps)
        {
            _steps = [.. steps];
        }

        public Task ExecuteAsync(TContext context, CancellationToken cancellationToken)
        {
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
}
