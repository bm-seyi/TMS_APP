using System.Diagnostics.CodeAnalysis;
using Duende.IdentityModel.OidcClient;
using TMS_APP.Models;
using IBrowser = Duende.IdentityModel.OidcClient.Browser.IBrowser;


namespace TMS_APP.OIDC
{
    public interface IAuthClient
    {
        Task<AuthResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
        IBrowser Browser { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class AuthClient : IAuthClient
    {
        private readonly OidcClient _client;

        public AuthClient(OidcClient client)
        {
            _client = client;
        }

        public async Task<AuthResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            LoginResult result = await _client.LoginAsync(request, cancellationToken);

            return new AuthResult
            {
                IsError = result.IsError,
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken,
                IdentityToken = result.IdentityToken,
                AccessTokenExpiration = result.AccessTokenExpiration,
                Error = result.Error,
                ErrorDescription = result.ErrorDescription
            };

        }

        public IBrowser Browser
        {
            get => _client.Options.Browser;
            set => _client.Options.Browser = value;
        }
    }
}


