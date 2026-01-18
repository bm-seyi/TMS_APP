using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;

namespace TMS.Core.Handlers
{
    public sealed class TmsAuthHeaderHandler : DelegatingHandler
    {
        private readonly IPublicClientApplication _publicClientApplication;
        private readonly IConfiguration _configuration;

        public TmsAuthHeaderHandler(IPublicClientApplication publicClientApplication, IConfiguration configuration)
        {
            _publicClientApplication = publicClientApplication ?? throw new ArgumentNullException(nameof(publicClientApplication));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accounts = await _publicClientApplication.GetAccountsAsync();
            var account = accounts.FirstOrDefault();

            string[]? scopes = _configuration.GetSection("AzureAD:Scopes").Get<string[]>();

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

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", result.AccessToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }

}