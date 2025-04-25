using System.Net;
using IdentityModel.OidcClient.Browser;
using IBrowser = IdentityModel.OidcClient.Browser.IBrowser;

namespace TMS_APP.AccessControl
{
    public class BrowserService : IBrowser
    {
        public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
        {
            string redirectUri = "http://localhost:5000/callback/";
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(redirectUri);
            listener.Start();

            try
            {
                // Open the browser for the user to authenticate
                await Launcher.OpenAsync(new Uri(options.StartUrl));

                // Create a task for handling incoming HTTP requests
                Task<HttpListenerContext> getContextTask = listener.GetContextAsync();

                // Create a task that completes when the token is canceled
                Task cancellationTask = Task.Delay(Timeout.Infinite, cancellationToken);

                // Wait for either the HTTP request or the cancellation
                Task completedTask = await Task.WhenAny(getContextTask, cancellationTask);

                if (completedTask == cancellationTask)
                {
                    return new BrowserResult
                    {
                        ResultType = BrowserResultType.UserCancel,
                        Error = "Login was canceled."
                    };
                }

                HttpListenerContext context = await getContextTask;

                if (context.Request.Url == null) throw new ArgumentNullException("Callback URL is null");

                string callbackUrl = context.Request.Url.ToString();

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
                listener.Close();
            }
        }

    }
}
