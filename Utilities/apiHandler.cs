using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Headers;
using System;
using System.Threading.Tasks;

namespace TMS_APP.Utilities;

public class ApiUtilities 
{
    public static async Task<int> PostDataToAPI(Dictionary<string, string> data, string endpoint)
	{	
		string url = "http://localhost:5188/";
		string JSON_data = JsonSerializer.Serialize(data);
		string type = "application/json";

		using (HttpClient client = new HttpClient()) 
		{
			string apikeyvalue = Environment.GetEnvironmentVariable("apiKey") ?? throw new ArgumentNullException("apiKey cannot be null", nameof(apikeyvalue));
			client.BaseAddress = new Uri(url);
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(type));
			client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", type);
			client.DefaultRequestHeaders.Add("X-API-Key", Environment.GetEnvironmentVariable("apiKey"));

			var payload = new StringContent(JSON_data, System.Text.Encoding.UTF8, type);

			try
			{
				var response = await client.PostAsync(endpoint, payload);

				if (response.IsSuccessStatusCode)
				{
					var response_content = await response.Content.ReadAsStringAsync();
					return (int)response.StatusCode;
				}

			} catch (Exception ex)
			{
				Console.WriteLine($"Exception: {ex.Message}");

				if (ex.InnerException != null)
				{
					Console.WriteLine($"Error: {ex.InnerException.Message}");
				}
			}

			return -1;
		}
	}
}