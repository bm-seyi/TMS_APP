using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Headers;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text.Json.Nodes;
using System.Net.Http.Json;
using TMS_APP.Utilities.API;

namespace TMS_APP.Utilities.API
{
	public class ApiUtilities : IApiUtilities
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ILogger<ApiUtilities> _logger;
		private readonly string _apiKey;

		public ApiUtilities(IHttpClientFactory httpClientFactory, ILogger<ApiUtilities> logger)
		{
			if (httpClientFactory == null) throw new ArgumentNullException(nameof(httpClientFactory));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));

			_httpClientFactory = httpClientFactory;

			_apiKey = Environment.GetEnvironmentVariable("apiKey") ?? throw new ArgumentNullException(nameof(_apiKey));
			if (string.IsNullOrWhiteSpace(_apiKey))
			{
				const string error_msg = "API Key cannot be null, empty or whitespace";
				_logger.LogError(error_msg);
				throw new ArgumentNullException(error_msg);
			}

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
			
			client.DefaultRequestHeaders.Add("X-API-KEY", _apiKey);
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

	public interface IApiUtilities
	{
		Task<int> PostDataToAPI(Dictionary<string, string> data, string endpoint);
		Task<T> GetDataFromAPI<T>(string url, string endpoint, Dictionary<string, string> param);
	}

}