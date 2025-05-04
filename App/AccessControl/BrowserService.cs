using CommunityToolkit.Mvvm.Messaging;
using IdentityModel.OidcClient.Browser;
using TMS_APP.InternalMessages;
using IBrowser = IdentityModel.OidcClient.Browser.IBrowser;

namespace TMS_APP.AccessControl
{
    public class BrowserService : IBrowser
    {
        private TaskCompletionSource<BrowserResult>? _tcs;

        public BrowserService()
        {
            // Register for protocol URI messages
            WeakReferenceMessenger.Default.Register<BrowserService, ProtocolUriMessage>(this, (r, m) =>
            {
                r.ReceiveProtocolUri(m.Value);
            });
        }

        public Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
        {
            _tcs = new TaskCompletionSource<BrowserResult>();

            try
            {
                Launcher.Default.OpenAsync(options.StartUrl);
            }
            catch (Exception ex)
            {
                _tcs.TrySetResult(new BrowserResult
                {
                    ResultType = BrowserResultType.UnknownError,
                    Error = ex.ToString()
                });
            }

            return _tcs.Task;
        }

        private void ReceiveProtocolUri(string uri)
        {
            _tcs?.TrySetResult(new BrowserResult
            {
                Response = uri,
                ResultType = BrowserResultType.Success
            });
        }
    }
}
