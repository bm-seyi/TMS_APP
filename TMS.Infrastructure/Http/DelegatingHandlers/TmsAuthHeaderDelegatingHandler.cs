using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using TMS.Domain.Configuration;

namespace TMS.Infrastructure.Http.DelegatingHandlers;


internal sealed class TmsAuthHeaderDelegatingHandler(ILogger<TmsAuthHeaderDelegatingHandler> logger, IPublicClientApplication publicClientApplication, IOptions<AzureAdOptions> options) : DelegatingHandler
{
    private readonly ILogger<TmsAuthHeaderDelegatingHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IPublicClientApplication _publicClientApplication = publicClientApplication ?? throw new ArgumentNullException(nameof(publicClientApplication));
    private readonly IOptions<AzureAdOptions> _options = options ?? throw new ArgumentNullException(nameof(options));

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("MSAL handler starting request. Method={Method} Uri={Uri}", request.Method, request.RequestUri);
        IEnumerable<IAccount> accounts = await _publicClientApplication.GetAccountsAsync();
        IAccount? account = accounts.FirstOrDefault();

        _logger.LogInformation("Retrieved MSAL accounts. Count={Count} UsingAccount={HasAccount}", accounts.Count(), account != null);

        string[] scopes = _options.Value.Scopes;

        _logger.LogInformation("Using scopes. Scopes={Scopes}", string.Join(", ", scopes));

        AuthenticationResult result;

        try
        {
            _logger.LogInformation("Attempting AcquireTokenSilent.");
            result = await _publicClientApplication
                .AcquireTokenSilent(scopes, account)
                .ExecuteAsync(cancellationToken);

            _logger.LogInformation("AcquireTokenSilent succeeded. ExpiresOn={ExpiresOn}", result.ExpiresOn);
        }
        catch (MsalUiRequiredException)
        {
            _logger.LogWarning("AcquireTokenSilent failed. UI interaction required. Falling back to AcquireTokenInteractive.");

            result = await _publicClientApplication
                .AcquireTokenInteractive(scopes)
                .ExecuteAsync(cancellationToken);
            
            _logger.LogInformation("AcquireTokenInteractive succeeded. ExpiresOn={ExpiresOn}", result.ExpiresOn);
        }

        
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

        _logger.LogInformation("Authorization header set. Sending request.");

        return await base.SendAsync(request, cancellationToken);
    }
}