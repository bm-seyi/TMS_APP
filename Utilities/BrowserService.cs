using System.Net;
using System.Security;
using IdentityModel.OidcClient.Browser;
using IBrowser = IdentityModel.OidcClient.Browser.IBrowser;

namespace TMS_APP.Utilities
{
    public class BrowserService : IBrowser
    {
        public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
        {
            // Define the redirect URI and start the listener
            string redirectUri = "http://localhost:5000/callback/";
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(redirectUri);
            listener.Start();

            try
            {
                // Open the browser for the user to authenticate
                await Launcher.OpenAsync(new Uri(options.StartUrl));

                // Wait for the browser to redirect back with the authorization code
                HttpListenerContext context = await listener.GetContextAsync();
                
                if (context.Request.Url == null) throw new ArgumentNullException("Callback URL is null");

                string callbackUrl = context.Request.Url.ToString();

                string? returnedState = context.Request.QueryString["state"];

                if (string.IsNullOrWhiteSpace(returnedState))
                {
                    throw new SecurityException("State parameter missing in the response.");
                }

                return new BrowserResult
                {
                    ResultType = BrowserResultType.Success,
                    Response = callbackUrl
                };
            }
            catch (Exception ex)
            {
                return new BrowserResult
                {
                    ResultType = BrowserResultType.UnknownError,
                    Error = ex.Message
                };
            }
            finally
            {
                listener.Stop();
            }
        }
    }
}
