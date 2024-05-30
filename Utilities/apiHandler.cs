using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Headers;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TMS_APP.Utilities
{
	public class ApiUtilities 
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger<ApiUtilities> _logger;

		public ApiUtilities(HttpClient httpClient, ILogger<ApiUtilities> logger)
		{
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<int> PostDataToAPI(Dictionary<string, string> data, string endpoint)
		{	
			string url = "http://localhost:5188/";
			string JSON_data = JsonSerializer.Serialize(data);
			string type = "application/json";
			
			_httpClient.BaseAddress = new Uri(url);
			_httpClient.DefaultRequestHeaders.Accept.Clear();
			_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(type));
			_httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", type);

			string? apiKey = Environment.GetEnvironmentVariable("apiKey");
			if (string.IsNullOrWhiteSpace(apiKey))
			{
				string error_msg = "API Key cannot be null, empty or whitespace";
				_logger.LogError(error_msg);
				throw new ArgumentNullException(error_msg);
			}

			_httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey);

			StringContent payload = new StringContent(JSON_data, System.Text.Encoding.UTF8, type);

			try
			{
				HttpResponseMessage response = await _httpClient.PostAsync(endpoint, payload);

				if (response.IsSuccessStatusCode)
				{
					string response_content = await response.Content.ReadAsStringAsync();
					_logger.LogInformation("API call successful: {response_content}", response_content);
					return (int)response.StatusCode;
				}
				else
				{
					_logger.LogError("API call failed with status code: {StatusCode}", response.StatusCode);
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
	}
}