using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using TMS.Domain.Configuration;

namespace TMS.Infrastructure.Http.DelegatingHandlers;


public sealed class TmsAuthHeaderDelegatingHandler(ILogger<TmsAuthHeaderDelegatingHandler> logger, IPublicClientApplication publicClientApplication, IOptions<AzureAdOptions> options) : DelegatingHandler
{
    private readonly ILogger<TmsAuthHeaderDelegatingHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IPublicClientApplication _publicClientApplication = publicClientApplication ?? throw new ArgumentNullException(nameof(publicClientApplication));
    private readonly IOptions<AzureAdOptions> _options = options ?? throw new ArgumentNullException(nameof(options));

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        IEnumerable<IAccount> accounts = await _publicClientApplication.GetAccountsAsync();
        IAccount? account = accounts.FirstOrDefault();

        string[] scopes = _options.Value.Scopes;

        AuthenticationResult result;

        try
        {
            result = await _publicClientApplication
                .AcquireTokenSilent(scopes, account)
                .ExecuteAsync(cancellationToken);
        }
        catch (MsalUiRequiredException)
        {
            result = await _publicClientApplication
                .AcquireTokenInteractive(scopes)
                .ExecuteAsync(cancellationToken);
        }

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

        return await base.SendAsync(request, cancellationToken);
    }
}