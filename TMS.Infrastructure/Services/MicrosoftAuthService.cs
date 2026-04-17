using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using TMS.Core.Interfaces.Services;
using TMS.Domain.Configuration;


namespace TMS.Infrastructure.Services
{
    internal sealed class MicrosoftAuthService(ILogger<MicrosoftAuthService> logger, IOptions<AzureAdOptions> options, IPublicClientApplication publicClientApplication) : IMicrosoftAuthService
    {
        private readonly ILogger<MicrosoftAuthService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IOptions<AzureAdOptions> _options = options ?? throw new ArgumentNullException(nameof(options));
        private readonly IPublicClientApplication _publicClientApplication = publicClientApplication ?? throw new ArgumentNullException(nameof(publicClientApplication));
        private static readonly ActivitySource _activitySource = new ActivitySource("TMS.Infrastructure");

        public async Task<AuthenticationResult> LoginAsync(CancellationToken cancellationToken)
        {
            using Activity? activity = _activitySource.StartActivity("MicrosoftAuthService.LoginAsync");

            _logger.LogInformation("Starting interactive login flow");
       
            AuthenticationResult result = await _publicClientApplication
                .AcquireTokenInteractive(_options.Value.Scopes)
                .WithPrompt(Prompt.ForceLogin)
                .ExecuteAsync(cancellationToken);

            _logger.LogInformation("Login completed successfully for user: {User}", result.Account?.Username);
            
            return result;
        }
    }
}

