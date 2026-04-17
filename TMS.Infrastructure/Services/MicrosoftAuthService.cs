using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using TMS.Core.Interfaces.Services;
using TMS.Domain.Configuration;
using System.Diagnostics;
using TMS.Domain;


namespace TMS.Infrastructure.Services
{
    internal sealed class MicrosoftAuthService(ILogger<MicrosoftAuthService> logger, IOptions<AzureAdOptions> options, IPublicClientApplication publicClientApplication) : IMicrosoftAuthService
    {
        private readonly ILogger<MicrosoftAuthService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IOptions<AzureAdOptions> _options = options ?? throw new ArgumentNullException(nameof(options));
        private readonly IPublicClientApplication _publicClientApplication = publicClientApplication ?? throw new ArgumentNullException(nameof(publicClientApplication));
        private static readonly ActivitySource _activitySource = new ActivitySource("TMS.Infrastructure");

        public async Task<AuthenticatedUser> LoginAsync(CancellationToken cancellationToken)
        {
            using Activity? activity = _activitySource.StartActivity("MicrosoftAuthService.LoginAsync");

            _logger.LogInformation("Starting interactive login flow");
       
            AuthenticationResult result = await _publicClientApplication
                .AcquireTokenInteractive(_options.Value.Scopes)
                .WithPrompt(Prompt.ForceLogin)
                .ExecuteAsync(cancellationToken);

            AuthenticatedUser authenticatedUser = new AuthenticatedUser()
            {
                UserId = result.Account.HomeAccountId.Identifier,
                Email = result.Account.Username,
                AccessToken = result.AccessToken,
                ExpiresAt = result.ExpiresOn.UtcDateTime,
            };
            
            _logger.LogInformation("Login completed successfully for user: {User}", result.Account?.Username);
            
            return authenticatedUser;
        }
    }
}

