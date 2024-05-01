using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Headers;

namespace TMS_APP.Utilities;

public class API_Utilities 
{
    public static async void PostDataToAPI(Dictionary<string, string> data, string endpoint)
	{
		using (HttpClient client = new HttpClient()) 
		{
			string url = "http://localhost:5188/";

			string JSON_data = JsonSerializer.Serialize(data);
			client.BaseAddress = new Uri(url);
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

			var payload = new StringContent(JSON_data, System.Text.Encoding.UTF8, "application/json");

			var response = await client.PostAsync(endpoint, payload);

			if (response.IsSuccessStatusCode)
			{
				var response_content = await response.Content.ReadAsStringAsync();
				Console.WriteLine(response_content);
			} else 
			{
				Console.WriteLine(response.StatusCode);
			}
		}
	}
}