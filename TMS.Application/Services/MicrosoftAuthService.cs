using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using TMS.Core.Interfaces.Services;


namespace TMS.Core.Services
{
    internal sealed class MicrosoftAuthService : IMicrosoftAuthService
    {
        private readonly ILogger<MicrosoftAuthService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IPublicClientApplication _publicClientApplication;
        private static readonly ActivitySource _activitySource = new ActivitySource("TMS.Core.Services.MicrosoftAuthService");
        public MicrosoftAuthService(ILogger<MicrosoftAuthService> logger, IConfiguration configuration, IPublicClientApplication publicClientApplication)
        {
           _logger = logger ?? throw new ArgumentNullException(nameof(logger));
           _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
           _publicClientApplication = publicClientApplication ?? throw new ArgumentNullException(nameof(publicClientApplication));
        }

        public async Task<AuthenticationResult> LoginAsync(CancellationToken cancellationToken)
        {
            using Activity? activity = _activitySource.StartActivity("MicrosoftAuthService.LoginAsync");

            _logger.LogInformation("Starting interactive login flow");
       
            AuthenticationResult result = await _publicClientApplication
                .AcquireTokenInteractive(_configuration.GetRequiredSection("AzureAD:Scopes").Get<string[]>())
                .WithPrompt(Prompt.ForceLogin)
                .ExecuteAsync(cancellationToken);

            _logger.LogInformation("Login completed successfully for user: {User}", result.Account?.Username);
            
            return result;
        }
    }
}

