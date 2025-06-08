using Microsoft.Extensions.Logging;
using Duende.IdentityModel.OidcClient;
using TMS_APP.OIDC;
using TMS_APP.Models;
using IBrowser = Duende.IdentityModel.OidcClient.Browser.IBrowser;


namespace TMS_APP.AccessControl
{
    public interface IAuthService
    {
        Task<AuthResult> LoginAsync(CancellationToken cancellationToken = default);
        bool IsAuthenticated { get; }
    }
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        public readonly IBrowser _browser;
        private readonly IAuthClient _authClient;
        private readonly ISecureStorage _secureStorage;
        private AuthResult? _loginResult;
        public AuthService(ILogger<AuthService> logger, IAuthClient authClient, IBrowser browser, ISecureStorage secureStorage)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _secureStorage = secureStorage ?? throw new ArgumentNullException(nameof(secureStorage));
            _authClient = authClient ?? throw new ArgumentNullException(nameof(authClient));
            _browser = browser ?? throw new ArgumentNullException(nameof(browser));
            _authClient.Browser = _browser;
        }

        public async Task<AuthResult> LoginAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _loginResult = await _authClient.LoginAsync(new LoginRequest(), cancellationToken);

                if (_loginResult.IsError)
                {
                    _logger.LogWarning(_loginResult.Error);
                    throw new Exception($"Login failed: {_loginResult.ErrorDescription}");
                }

                Task accessTokenTask = _secureStorage.SetAsync("access_token", _loginResult.AccessToken);
                Task refreshTokenTask = _secureStorage.SetAsync("refresh_token", _loginResult.RefreshToken);
                Task identityTokenTask = _secureStorage.SetAsync("identity_token", _loginResult.IdentityToken);

                await Task.WhenAll(accessTokenTask, refreshTokenTask, identityTokenTask);

                _logger.LogInformation("Successfully stored 'access_token' in secure storage.");
                _logger.LogInformation("Successfully stored 'refresh_token' in secure storage.");
                _logger.LogInformation("Successfully stored 'identity_token' in secure storage.");


                return _loginResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login.");
                throw;
            }

        }

        public bool IsAuthenticated => _loginResult?.AccessToken != null &&
                                 _loginResult.AccessTokenExpiration > DateTime.UtcNow;
    }
}