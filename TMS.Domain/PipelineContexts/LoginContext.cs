
namespace TMS.Domain.PipelineContexts
{
    public sealed class LoginContext
    {
        public bool IsAuthenticated { get; set; }
        public AuthenticationProvider AuthenticationProvider {get; set; }
    }
}