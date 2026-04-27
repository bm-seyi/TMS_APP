using System.Diagnostics;
using Microsoft.Extensions.Logging;
using TMS.Application.Interfaces.AuthenticationProviders;
using TMS.Application.Interfaces.Services;
using TMS.Domain;
using TMS.Domain.PipelineContexts;


namespace TMS.Application.AuthenticationProviders
{
    internal sealed class MicrosoftAuthenticationProvider(ILogger<MicrosoftAuthenticationProvider> logger, IMicrosoftAuthService microsoftAuthService) : IAuthenticationProvider
    {
        private readonly ILogger<MicrosoftAuthenticationProvider> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IMicrosoftAuthService _microsoftAuthService = microsoftAuthService ?? throw new ArgumentNullException(nameof(microsoftAuthService));
        private static readonly ActivitySource _activitySource = new ActivitySource("TMS.Application");

        public AuthenticationProvider AuthenticationProvider => AuthenticationProvider.Microsoft;

        public async Task AuthenticateAsync(LoginContext loginContext, CancellationToken cancellationToken)
        {
            using Activity? activity = _activitySource.StartActivity("MicrosoftAuthenticationProvider.AuthenticationAsync");

            _logger.LogInformation("Starting Microsoft authentication");

            AuthenticatedUser authenticatedUser = await _microsoftAuthService.LoginAsync(cancellationToken);

            loginContext.IsAuthenticated = authenticatedUser?.AccessToken != null;

            _logger.LogInformation("Microsoft authentication result: IsAuthenticated = {IsAuthenticated}", loginContext.IsAuthenticated);
        }
    }
}