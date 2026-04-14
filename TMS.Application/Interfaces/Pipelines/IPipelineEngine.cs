namespace TMS.Core.Interfaces.Pipelines
{
    public interface IPipelineEngine<TContext>
    {
        Task ExecuteAsync(TContext context, CancellationToken cancellationToken);
    }
}