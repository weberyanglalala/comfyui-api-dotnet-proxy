using System.Net.WebSockets;
using System.Text;
using Dotnet8LineRichMenu.Models.ComfyUIApi.Prompt;
using Dotnet8LineRichMenu.Models.Settings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Dotnet8LineRichMenu.Services;

public class SimpleTextPromptService
{
    private readonly string _endpoint;
    private readonly string _clientId;
    private readonly HttpClient _httpClient;

    public SimpleTextPromptService(SimpleTextPromptFlowSettings settings, IHttpClientFactory httpClientFactory)
    {
        _endpoint = settings.Endpoint;
        _clientId = settings.ClientId;
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<string> CreatePromptAsync(string positivePrompt, string negativePrompt)
    {
        var endpoint = $"{_endpoint}/prompt";

        // Load and deserialize JSON using Newtonsoft.Json
        var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "json", "SimpleTextPrompt.json");
        var jsonData = await File.ReadAllTextAsync(jsonFilePath);

        // Use JObject to access dynamic properties
        var jsonPrompt = JObject.Parse(jsonData);

        // Initialize the seed
        var random = new Random();
        jsonPrompt["3"]["inputs"]["seed"] = random.Next(1, 1000000);

        // Replace placeholders in text fields with actual values
        // Positive prompt
        if (jsonPrompt.ContainsKey("6"))
        {
            // Accessing dynamic properties using dictionary syntax
            jsonPrompt["6"]["inputs"]["text"] =
                string.Format((string)jsonPrompt["6"]["inputs"]["text"], positivePrompt);
        }

        // Negative prompt
        if (jsonPrompt.ContainsKey("7"))
        {
            // Accessing dynamic properties using dictionary syntax
            jsonPrompt["7"]["inputs"]["text"] =
                string.Format((string)jsonPrompt["7"]["inputs"]["text"], negativePrompt);
        }

        // Create the request object
        var request = new JObject();
        request["prompt"] = jsonPrompt;
        request["client_id"] = _clientId;

        // Send the request to the ComfyUI API
        var httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(endpoint, httpContent);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var responseContent = JsonSerializer.Deserialize<CreatePromptResponse>(responseString);
        return responseContent.PromptId;
    }

    public async Task<string> GetImageByPromptId(string promptId)
    {
        var endpoint = $"{_endpoint}/history/{promptId}";
        var response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Use JObject to access dynamic properties
        var responseJson = JObject.Parse(responseString);

        // Accessing dynamic properties using dictionary syntax
        var outputs = responseJson[promptId]["outputs"];
        var image = outputs["9"]["images"][0];
        var imageFileName = image["filename"];
        var imageSubfolder = image["subfolder"];
        var imageType = image["type"];

        // https://{{domain}}/view?filename=ComfyUI_00095_.png&subfolder=&type=output
        var result = $"{_endpoint}/view?subfolder={imageSubfolder}&type={imageType}&filename={imageFileName}";
        Console.WriteLine(result);
        return result;
    }

    public async Task<bool> GetPromptStatus(string promptId)
    {
        try
        {
            var endpoint = $"{_endpoint}/history/{promptId}";
            var response = await _httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var responseJson = JObject.Parse(responseString);
                return responseJson.HasValues;
            }

            return false;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"HTTP request failed: {ex.Message}");
            return false;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JSON parsing failed: {ex.Message}");
            return false;
        }
    }
    
    public async Task<bool> CheckPromptStatusWithRetry(string promptId, int maxRetries = 10, int retryDelayMs = 5000)
    {
        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            bool promptStatus = await GetPromptStatus(promptId);
        
            if (promptStatus)
            {
                return true;
            }
        
            if (attempt < maxRetries)
            {
                Console.WriteLine($"Attempt {attempt} failed. Retrying in {retryDelayMs/1000} seconds...");
                await Task.Delay(retryDelayMs);
            }
            else
            {
                Console.WriteLine($"Max retries ({maxRetries}) reached. Prompt status check failed.");
            }
        }
        return false;
    }
}