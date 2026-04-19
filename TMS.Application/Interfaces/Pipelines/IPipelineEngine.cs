namespace TMS.Application.Interfaces.Pipelines
{
    public interface IPipelineEngine<TContext>
    {
        Task ExecuteAsync(TContext context, CancellationToken cancellationToken);
    }
}