using Microsoft.Extensions.Logging;
using Duende.IdentityModel.OidcClient;
using IBrowser = Duende.IdentityModel.OidcClient.Browser.IBrowser;

namespace TMS_APP.AccessControl
{
    public interface IAuthService
    {
        Task LoginAsync(CancellationToken cancellationToken = default);
    }
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly IBrowser _browser;
        public AuthService(ILogger<AuthService> logger, IBrowser browser)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _browser = browser ?? throw new ArgumentNullException(nameof(browser));
        }

        public async Task LoginAsync(CancellationToken cancellationToken = default)
        {
            OidcClientOptions oidcClientOptions = new OidcClientOptions
            {
                Authority = "https://localhost:8443/realms/maui_realm",
                ClientId = "maui_client",
                Scope = "signalR.read offline_access",
                RedirectUri = "tmsapp://callback/",
                PostLogoutRedirectUri = "tmsapp://logout-callback/",
                Browser = _browser,
                DisablePushedAuthorization = false,
            };

            OidcClient oidcClient = new OidcClient(oidcClientOptions);

            LoginResult result = await oidcClient.LoginAsync(new LoginRequest(), cancellationToken);

            if (result.IsError)
            {
                _logger.LogWarning(result.Error);
                throw new Exception($"Login failed: {result.ErrorDescription}");
            }

            Task accessTokenTask = SecureStorage.SetAsync("access_token", result.AccessToken);
            Task refreshTokenTask = SecureStorage.SetAsync("refresh_token", result.RefreshToken);
            Task identityTokenTask = SecureStorage.SetAsync("identity_token", result.IdentityToken);

            await Task.WhenAll(accessTokenTask, refreshTokenTask, identityTokenTask);

            _logger.LogInformation("Successfully stored 'access_token' in secure storage.");
            _logger.LogInformation("Successfully stored 'refresh_token' in secure storage.");
            _logger.LogInformation("Successfully stored 'identity_token' in secure storage.");
        }

    }
}