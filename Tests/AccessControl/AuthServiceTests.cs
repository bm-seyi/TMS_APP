using Microsoft.Extensions.Logging;
using Moq;
using IBrowser = Duende.IdentityModel.OidcClient.Browser.IBrowser;
using Duende.IdentityModel.OidcClient.Browser;
using Duende.IdentityModel.OidcClient;
using TMS_APP.AccessControl;
using TMS_APP.OIDC;
using TMS_APP.Models;


namespace TMS_APP.Tests.AccessControl
{
    [TestClass]
    public class AuthServiceTests
    {
        private Mock<ILogger<AuthService>> _loggerMock = null!;
        private Mock<IAuthClient> _oidcClient = null!;
        private Mock<ISecureStorage> _secureStorage = null!;
        private AuthService _authService = null!;

        [TestInitialize]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<AuthService>>();
            _oidcClient = new Mock<IAuthClient>();
            _secureStorage = new Mock<ISecureStorage>();
        }

        [TestMethod]
        public void Constructor_ShouldThrowArgumentNullException_WhenDependenciesAreNull()
        {
            MockBrowser mockBrowser = new MockBrowser(new BrowserResult
            {
                Response = "https://example.com/callback?code=12345",
                ResultType = BrowserResultType.Success
            });

            Assert.ThrowsException<ArgumentNullException>(() => new AuthService(null!,  _oidcClient.Object,mockBrowser, _secureStorage.Object));
            Assert.ThrowsException<ArgumentNullException>(() => new AuthService(_loggerMock.Object, null!,mockBrowser, _secureStorage.Object));
            Assert.ThrowsException<ArgumentNullException>(() => new AuthService(_loggerMock.Object, _oidcClient.Object,null!, _secureStorage.Object));
            Assert.ThrowsException<ArgumentNullException>(() => new AuthService(_loggerMock.Object, _oidcClient.Object,mockBrowser, null!));
        }

        [TestMethod]
        public async Task LoginAsync_WithSuccessfulBrowserResult_ReturnsSuccess()
        {
            MockBrowser mockBrowser = new MockBrowser(new BrowserResult
            {
                Response = "https://example.com/callback?code=12345",
                ResultType = BrowserResultType.Success
            });

            _authService = new AuthService(_loggerMock.Object, _oidcClient.Object, mockBrowser, _secureStorage.Object);

            _oidcClient.Setup(client => client.LoginAsync(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AuthResult
                {
                    IsError = false,
                    AccessToken = "mock_access_token",
                    RefreshToken = "mock_refresh_token",
                    IdentityToken = "mock_identity_token",
                    AccessTokenExpiration = DateTime.UtcNow.AddHours(1)
                });

            _secureStorage.Setup(s => s.SetAsync("access_token", "mock_access_token"));
            _secureStorage.Setup(s => s.SetAsync("refresh_token", "mock_refresh_token"));
            _secureStorage.Setup(s => s.SetAsync("identity_token", "mock_identity_token"));

            AuthResult AuthResult = await _authService.LoginAsync();

            Assert.IsFalse(AuthResult.IsError);
            Assert.IsNotNull(AuthResult.AccessToken);

            _secureStorage.Verify(s => s.SetAsync("access_token", "mock_access_token"), Times.Once);
            _secureStorage.Verify(s => s.SetAsync("refresh_token", "mock_refresh_token"), Times.Once);
            _secureStorage.Verify(s => s.SetAsync("identity_token", "mock_identity_token"), Times.Once);
        }

        [TestMethod]
        public async Task LoginAsync_WithFailedBrowserResult_ThrowsException()
        {
            MockBrowser mockBrowser = new MockBrowser(new BrowserResult
            {
                ResultType = BrowserResultType.HttpError,
                Error = "Connection failed"
            });

            _authService = new AuthService(_loggerMock.Object, _oidcClient.Object, mockBrowser, _secureStorage.Object);

            _oidcClient.Setup(client => client.LoginAsync(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Connection failed"));


            await Assert.ThrowsExceptionAsync<Exception>(async () => await _authService.LoginAsync());
            _loggerMock.Verify(
            logger => logger.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "An error occurred during login."),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
                Times.Once);
        }

        [TestMethod]
        public async Task LoginAsync_WithFailedOidcLogin_ThrowsException()
        {
            MockBrowser mockBrowser = new MockBrowser(new BrowserResult
            {
                Response = "https://example.com/callback?code=12345",
                ResultType = BrowserResultType.Success
            });

            _authService = new AuthService(_loggerMock.Object, _oidcClient.Object, mockBrowser, _secureStorage.Object);

            _oidcClient.Setup(client => client.LoginAsync(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AuthResult
                {
                    AccessToken = string.Empty,
                    RefreshToken = string.Empty,
                    IdentityToken = string.Empty,
                    IsError = true,
                    Error = "Invalid credentials",
                    ErrorDescription = "The provided credentials are invalid"
                });

            Exception exception = await Assert.ThrowsExceptionAsync<Exception>(() => _authService.LoginAsync());
            Assert.IsTrue(exception.Message.Contains("Login failed"));
            _loggerMock.Verify(
            logger => logger.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "Invalid credentials"),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
                Times.Once);
        }

        [TestMethod]
        public async Task LoginAsync_WithSecureStorageFailure_ThrowsException()
        {
            // Arrange
            var mockBrowser = new MockBrowser(new BrowserResult
            {
                Response = "https://example.com/callback?code=12345",
                ResultType = BrowserResultType.Success
            });

            _authService = new AuthService(_loggerMock.Object, _oidcClient.Object, mockBrowser, _secureStorage.Object);

            _oidcClient.Setup(client => client.LoginAsync(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AuthResult
                {
                    IsError = false,
                    AccessToken = "mock_access_token",
                    RefreshToken = "mock_refresh_token",
                    IdentityToken = "mock_identity_token",
                    AccessTokenExpiration = DateTime.UtcNow.AddHours(1)
                });

            _secureStorage.Setup(s => s.SetAsync("access_token", It.IsAny<string>()))
                .ThrowsAsync(new Exception("Storage failure"));

            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(() => _authService.LoginAsync());
        }

        [TestMethod]
        public async Task IsAuthenticated_WithValidToken_ReturnsTrue()
        {
            // Arrange
            var mockBrowser = new MockBrowser(new BrowserResult
            {
                Response = "https://example.com/callback?code=12345",
                ResultType = BrowserResultType.Success
            });

            _authService = new AuthService(_loggerMock.Object, _oidcClient.Object, mockBrowser, _secureStorage.Object);

            _oidcClient.Setup(client => client.LoginAsync(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AuthResult
                {
                    IsError = false,
                    AccessToken = "mock_access_token",
                    RefreshToken = "mock_refresh_token",
                    IdentityToken = "mock_identity_token",
                    AccessTokenExpiration = DateTime.UtcNow.AddHours(1)
                });

            await _authService.LoginAsync();
    
            // Assert
            Assert.IsTrue(_authService.IsAuthenticated);
        }

        [TestMethod]
        public async Task IsAuthenticated_WithExpiredToken_ReturnsFalse()
        {
            // Arrange
            var mockBrowser = new MockBrowser(new BrowserResult { ResultType = BrowserResultType.Success });
            _authService = new AuthService(_loggerMock.Object, _oidcClient.Object, mockBrowser, _secureStorage.Object);

            _authService = new AuthService(_loggerMock.Object, _oidcClient.Object, mockBrowser, _secureStorage.Object);

            _oidcClient.Setup(client => client.LoginAsync(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AuthResult
                {
                    IsError = false,
                    AccessToken = "mock_access_token",
                    RefreshToken = "mock_refresh_token",
                    IdentityToken = "mock_identity_token",
                    AccessTokenExpiration = DateTime.UtcNow.AddHours(-1)
                });

            await _authService.LoginAsync();
            // Assert
            Assert.IsFalse( _authService.IsAuthenticated);
        }

        [TestMethod]
        public async Task LoginAsync_WithCancellationToken_CancelsOperation()
        {
            // Arrange
            var mockBrowser = new MockBrowser(new BrowserResult { ResultType = BrowserResultType.Success });
            _authService = new AuthService(_loggerMock.Object, _oidcClient.Object, mockBrowser, _secureStorage.Object);

            var cts = new CancellationTokenSource();
            cts.Cancel();

            _oidcClient.Setup(client => client.LoginAsync(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new OperationCanceledException());

            // Act & Assert
            await Assert.ThrowsExceptionAsync<OperationCanceledException>(
                () => _authService.LoginAsync(cts.Token));
        }

    }



    public class MockBrowser : IBrowser
    {
        private BrowserResult _predefinedResult;

        public MockBrowser(BrowserResult predefinedResult)
        {
            _predefinedResult = predefinedResult;
        }

        public Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_predefinedResult);
        }
    }
    
}