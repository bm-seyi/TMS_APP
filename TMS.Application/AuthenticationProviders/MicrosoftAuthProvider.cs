using System.Diagnostics;
using Microsoft.Extensions.Logging;
using TMS.Application.Interfaces.AuthenticationProviders;
using TMS.Application.Interfaces.Services;
using TMS.Domain;
using TMS.Domain.PipelineContexts;


namespace TMS.Application.AuthenticationProviders
{
    internal sealed class MicrosoftAuthProvider(ILogger<MicrosoftAuthProvider> logger, IMicrosoftAuthService microsoftAuthService) : IAuthenticationProvider
    {
        private readonly ILogger<MicrosoftAuthProvider> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IMicrosoftAuthService _microsoftAuthService = microsoftAuthService ?? throw new ArgumentNullException(nameof(microsoftAuthService));
        private static readonly ActivitySource _activitySource = new ActivitySource("TMS.Application");

        public AuthenticationProvider AuthenticationProvider => AuthenticationProvider.Microsoft;

        public async Task AuthenticateAsync(LoginContext loginContext, CancellationToken cancellationToken)
        {
            using Activity? activity = _activitySource.StartActivity("MicrosoftProvider.AuthenticationAsync");

            _logger.LogInformation("Starting Microsoft authentication");

            AuthenticatedUser authenticatedUser = await _microsoftAuthService.LoginAsync(cancellationToken);

            _logger.LogDebug("Microsoft authentication completed. AccessToken null: {IsNull}", authenticatedUser?.AccessToken == null);

            loginContext.IsAuthenticated = authenticatedUser?.AccessToken != null;

            _logger.LogInformation("Microsoft authentication result: IsAuthenticated = {IsAuthenticated}", loginContext.IsAuthenticated);
        }
    }
}