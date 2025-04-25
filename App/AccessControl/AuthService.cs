using IdentityModel.OidcClient;
using IBrowser = IdentityModel.OidcClient.Browser.IBrowser;
using Microsoft.Extensions.Logging;
using TMS_APP.Models;
using System.Net.Http.Json;

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
                Scope = "openid profile api1.read offline_access",
                RedirectUri = "http://localhost:5000/callback",
                PostLogoutRedirectUri = "http://localhost:5000/signout-callback",
                Browser = _browser
            };

            OidcClient oidcClient = new OidcClient(oidcClientOptions);

            LoginResult result = await oidcClient.LoginAsync(new LoginRequest());

            if (result.IsError)
            {
                _logger.LogWarning(result.Error);
                throw new Exception($"Login failed: {result.ErrorDescription}");
            }

            await SecureStorage.SetAsync("access_token", result.AccessToken);
            _logger.LogInformation("Successfully stored 'access_token' in secure storage.");

            await SecureStorage.SetAsync("refresh_token", result.RefreshToken);
            _logger.LogInformation("Successfully stored 'refresh_token' in secure storage.");

            await SecureStorage.SetAsync("identity_token", result.IdentityToken);
            _logger.LogInformation("Successfully stored 'identity_token' in secure storage.");
        }

        // TODO: Implement the registration logic here
    }
}