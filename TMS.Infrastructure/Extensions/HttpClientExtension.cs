using System.Net.Http.Json;

namespace TMS.Infrastructure.Extensions
{
    public static class HttpClientExtension
    {
        extension(HttpClient httpClient)
        {
            public async Task<TModel> GetRequiredFromJsonAsync<TModel>(string? requestUri, CancellationToken cancellationToken = default)
            {
                TModel data = await httpClient.GetFromJsonAsync<TModel>(requestUri, cancellationToken) ?? throw new InvalidOperationException($"Request to '{requestUri}' returned no data or could not be deserialized into '{typeof(TModel).Name}'.");
                return data;
            }
        }
    }
}