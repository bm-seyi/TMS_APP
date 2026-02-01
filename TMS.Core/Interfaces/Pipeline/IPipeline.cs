namespace TMS.Core.Interfaces.Pipeline
{
    public interface IPipeline<TContext>
    {
        Task ExecuteAsync(TContext context, CancellationToken cancellationToken);
    }
}