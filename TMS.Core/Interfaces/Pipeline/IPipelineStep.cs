namespace TMS.Core.Interfaces.Pipeline
{
    public interface IPipelineStep<TContext>
    {
        Task InvokeAsync(TContext context, Func<Task> next, CancellationToken cancellationToken);
    }
}