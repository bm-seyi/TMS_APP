using IdentityModel.OidcClient;
using IBrowser = IdentityModel.OidcClient.Browser.IBrowser;
using Microsoft.Extensions.Logging;

namespace TMS_APP.AccessControl
{
    public interface IAuthService
    {
        Task LoginAsync();
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

        public async Task LoginAsync()
        {
            OidcClientOptions oidcClientOptions = new OidcClientOptions
            {
                Authority = "https://localhost:5188",
                ClientId = "maui_client",
                Scope = "openid profile signalR.read offline_access",
                RedirectUri = "tmsapp://callback/",
                PostLogoutRedirectUri = "tmsapp://logout-callback/",
                Browser = _browser
            };

            OidcClient oidcClient = new OidcClient(oidcClientOptions);

            LoginResult result = await oidcClient.LoginAsync(new LoginRequest());

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

        //TODO Registration Logic
    }
}