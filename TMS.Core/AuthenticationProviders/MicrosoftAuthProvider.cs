using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using TMS.Core.Interfaces.AuthenticationProviders;
using TMS.Core.Interfaces.Services;
using TMS.Models;
using TMS.Models.PipelineContexts;


namespace TMS.Core.AuthenticationProviders
{
    internal sealed class MicrosoftAuthProvider : IAuthenticationProvider
    {
        private readonly ILogger<MicrosoftAuthProvider> _logger;
        private readonly IMicrosoftAuthService _microsoftAuthService;
        private static readonly ActivitySource _activitySource = new ActivitySource("TMS.Core.AuthenticationProviders.MicrosoftProvider");

        public MicrosoftAuthProvider(ILogger<MicrosoftAuthProvider> logger, IMicrosoftAuthService microsoftAuthService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _microsoftAuthService  = microsoftAuthService ?? throw new ArgumentNullException(nameof(microsoftAuthService));
        }

        public AuthenticationProvider AuthenticationProvider => AuthenticationProvider.Microsoft;

        public async Task AuthenticateAsync(LoginContext loginContext, CancellationToken cancellationToken)
        {
            using Activity? activity = _activitySource.StartActivity("MicrosoftProvider.AuthenticationAsync");

            _logger.LogInformation("Starting Microsoft authentication");

            AuthenticationResult authenticationResult = await _microsoftAuthService.LoginAsync(cancellationToken);

            _logger.LogDebug("Microsoft authentication completed. AccessToken null: {IsNull}", authenticationResult?.AccessToken == null);

            loginContext.IsAuthenticated = authenticationResult?.AccessToken != null;

            _logger.LogInformation("Microsoft authentication result: IsAuthenticated = {IsAuthenticated}", loginContext.IsAuthenticated);

        }
    }
}