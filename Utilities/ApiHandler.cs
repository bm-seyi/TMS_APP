using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace TMS_APP.Utilities
{
	public interface IApiUtilities
	{
		Task<int> PostDataToAPI(Dictionary<string, string> data, string endpoint);
		Task<T> GetDataFromAPI<T>(string url, string endpoint, Dictionary<string, string> param);
	}

	public class ApiUtilities : IApiUtilities
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ILogger<ApiUtilities> _logger;
		string apiKey = Environment.GetEnvironmentVariable("apiKey") ?? throw new ArgumentNullException(nameof(apiKey));
		public ApiUtilities(IHttpClientFactory httpClientFactory, ILogger<ApiUtilities> logger)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));

			_httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));

		}

		private HttpClient CreateHttpClient (string baseAddress)
		{
			HttpClient client = _httpClientFactory.CreateClient();
			client.BaseAddress = new Uri(baseAddress);
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
			client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
			{
				NoCache = true,
				NoStore = true,
				MustRevalidate = true
			};

			return client;
		}

		public async Task<int> PostDataToAPI(Dictionary<string, string> data, string endpoint)
		{	
			using HttpClient client = CreateHttpClient("http://localhost:5188/");
			
			client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
			string JSON_data = JsonSerializer.Serialize(data);

			StringContent payload = new StringContent(JSON_data, System.Text.Encoding.UTF8, "application/json");

			try
			{
				HttpResponseMessage response = await client.PostAsync(endpoint, payload);

				if (response.IsSuccessStatusCode)
				{
					string response_content = await response.Content.ReadAsStringAsync();
					_logger.LogInformation("API call successful: {response_content}", response_content);
				
					return (int)response.StatusCode;
				}
				else
				{
					_logger.LogError("API call failed with status code: {StatusCode}", response.StatusCode);	
					client.DefaultRequestHeaders.Clear();
					_logger.LogInformation("Request Header Has Been Cleared");
				}

			} 
			catch (HttpRequestException httpRequestException)
			{
				_logger.LogError("Request Error: {Message}", httpRequestException.Message);

				if (httpRequestException.InnerException != null)
				{
					_logger.LogError("Inner Exception: {Message}", httpRequestException.InnerException.Message);
				}
			}
			
			catch (Exception ex)
			{
				_logger.LogError("Unexpected Error: {Message}", ex.Message);

				if (ex.InnerException != null)
				{
					_logger.LogError("Error: {Message}", ex.InnerException.Message);
				}
			}

			return -1;
			
		}

		public async Task<T> GetDataFromAPI<T>(string url, string endpoint, Dictionary<string, string> param)
		{

			if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));
			if (string.IsNullOrWhiteSpace(endpoint)) throw new ArgumentNullException(nameof(endpoint));
			if (param == null) throw new ArgumentNullException(nameof(param));

			using (var httpClient = CreateHttpClient(url))
			{
				string apiQuery = await new FormUrlEncodedContent(param).ReadAsStringAsync();
				string apiUrl = $"{endpoint}?{apiQuery}";

				return await httpClient.GetFromJsonAsync<T>(apiUrl) ?? throw new Exception("Failed to retrieve data from API");
			}

		}
	}
}