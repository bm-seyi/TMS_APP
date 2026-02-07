namespace TMS.Core.Interfaces.Pipelines
{
    public interface IPipelineStep<TContext>
    {
        Task InvokeAsync(TContext context, Func<Task> next, CancellationToken cancellationToken);
    }
}