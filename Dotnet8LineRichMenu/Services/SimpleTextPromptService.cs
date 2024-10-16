using System.Net.WebSockets;
using System.Text;
using Dotnet8LineRichMenu.Models.ComfyUIApi.Prompt;
using Dotnet8LineRichMenu.Models.Dtos.ComfyUI;
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

    public async Task<List<string>> GetCharacterPhotosByPromptId(string promptId)
    {
        var endpoint = $"{_endpoint}/history/{promptId}";
        var response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Use JObject to access dynamic properties
        var responseJson = JObject.Parse(responseString);

        // result
        var imageList = new List<string>();
        // Accessing dynamic properties using dictionary syntax
        var outputs = responseJson[promptId]["outputs"];
        var images = outputs["12"]["images"];
        foreach (var image in images)
        {
            var imageFileName = image["filename"];
            var imageSubfolder = image["subfolder"];
            var imageType = image["type"];
            var result = $"{_endpoint}/view?subfolder={imageSubfolder}&type={imageType}&filename={imageFileName}";
            imageList.Add(result);
        }

        return imageList;
    }


    public async Task<string> CreateCharacterPhoto(string imageName, string style, int seed, int count = 4, int width = 864,
        int height = 1534)
    {
        var endpoint = $"{_endpoint}/prompt";
        // Load and deserialize JSON using Newtonsoft.Json
        var jsonFilePath =
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "json", "CharacterPhoto.json");
        var jsonData = await File.ReadAllTextAsync(jsonFilePath);
        // Use JObject to access dynamic properties
        var jsonPrompt = JObject.Parse(jsonData);

        // Initialize the seed
        if (jsonPrompt.ContainsKey("7"))
        {
            // Accessing dynamic properties using dictionary syntax
            jsonPrompt["7"]["inputs"]["noise_seed"] = seed;
        }

        // get upload image path
        if (jsonPrompt.ContainsKey("22"))
        {
            // Accessing dynamic properties using dictionary syntax
            jsonPrompt["22"]["inputs"]["image"] = imageName;
        }

        // setup style
        if (jsonPrompt.ContainsKey("4"))
        {
            // Accessing dynamic properties using dictionary syntax
            jsonPrompt["4"]["inputs"]["text"] = style;
        }
        
        // setup count
        if (jsonPrompt.ContainsKey("6"))
        {
            // Accessing dynamic properties using dictionary syntax
            jsonPrompt["6"]["inputs"]["batch_size"] = count;
            jsonPrompt["6"]["inputs"]["width"] = width;
            jsonPrompt["6"]["inputs"]["height"] = height;
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

    public async Task<string> GetFurnitureDesignImageByPromptId(string promptId)
    {
        var endpoint = $"{_endpoint}/history/{promptId}";
        var response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Use JObject to access dynamic properties
        var responseJson = JObject.Parse(responseString);

        // Accessing dynamic properties using dictionary syntax
        var outputs = responseJson[promptId]["outputs"];
        var image = outputs["48"]["images"][0];
        var imageFileName = image["filename"];
        var imageSubfolder = image["subfolder"];
        var imageType = image["type"];

        // https://{{domain}}/view?filename=ComfyUI_00095_.png&subfolder=&type=output
        var result = $"{_endpoint}/view?subfolder={imageSubfolder}&type={imageType}&filename={imageFileName}";
        return result;
    }

    public async Task<string> CreateFurnitureDesign(string imageName, string style, int seed)
    {
        var endpoint = $"{_endpoint}/prompt";

        // Load and deserialize JSON using Newtonsoft.Json
        var jsonFilePath =
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "json", "ModernFurnitureDesign.json");
        var jsonData = await File.ReadAllTextAsync(jsonFilePath);

        // Use JObject to access dynamic properties
        var jsonPrompt = JObject.Parse(jsonData);

        // Initialize the seed
        if (jsonPrompt.ContainsKey("10"))
        {
            // Accessing dynamic properties using dictionary syntax
            jsonPrompt["10"]["inputs"]["seed"] = seed;
        }

        // get upload image path
        if (jsonPrompt.ContainsKey("19"))
        {
            // Accessing dynamic properties using dictionary syntax
            jsonPrompt["19"]["inputs"]["image"] = imageName;
        }

        // setup style
        if (jsonPrompt.ContainsKey("78"))
        {
            // Accessing dynamic properties using dictionary syntax
            jsonPrompt["78"]["inputs"]["text"] = style;
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

    public async Task<string> GetFluxStyleChangeImageByPromptId(string promptId)
    {
        var endpoint = $"{_endpoint}/history/{promptId}";
        var response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Use JObject to access dynamic properties
        var responseJson = JObject.Parse(responseString);

        // Accessing dynamic properties using dictionary syntax
        var outputs = responseJson[promptId]["outputs"];
        var image = outputs["12"]["images"][0];
        var imageFileName = image["filename"];
        var imageSubfolder = image["subfolder"];
        var imageType = image["type"];

        // https://{{domain}}/view?filename=ComfyUI_00095_.png&subfolder=&type=output
        var result = $"{_endpoint}/view?subfolder={imageSubfolder}&type={imageType}&filename={imageFileName}";
        return result;
    }

    public async Task<string> CreateFluxStyleChangeImageWithStyleAndSeed(string image, string style, int seed,
        int width, int height)
    {
        var endpoint = $"{_endpoint}/prompt";

        // Load and deserialize JSON using Newtonsoft.Json
        var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "json", "FluxStyleChange.json");
        var jsonData = await File.ReadAllTextAsync(jsonFilePath);

        // Use JObject to access dynamic properties
        var jsonPrompt = JObject.Parse(jsonData);

        // Initialize the seed
        if (jsonPrompt.ContainsKey("7"))
        {
            // Accessing dynamic properties using dictionary syntax
            jsonPrompt["7"]["inputs"]["noise_seed"] = seed;
        }

        // get upload image path
        if (jsonPrompt.ContainsKey("22"))
        {
            jsonPrompt["22"]["inputs"]["image"] = image;
        }

        // setup style
        if (jsonPrompt.ContainsKey("4"))
        {
            jsonPrompt["4"]["inputs"]["text"] = style;
        }

        // setup width and height
        if (jsonPrompt.ContainsKey("6"))
        {
            jsonPrompt["6"]["inputs"]["width"] = width;
            jsonPrompt["6"]["inputs"]["height"] = height;
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


    public async Task<string> CreateFluxPromptWithSeedAndCustomPromptAndSize(string prompt, int seed, int width,
        int height)
    {
        var endpoint = $"{_endpoint}/prompt";

        // Load and deserialize JSON using Newtonsoft.Json
        var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "json", "FluxOptimizedPrompt.json");
        var jsonData = await File.ReadAllTextAsync(jsonFilePath);

        // Use JObject to access dynamic properties
        var jsonPrompt = JObject.Parse(jsonData);

        // Initialize the seed
        if (jsonPrompt.ContainsKey("6"))
        {
            // Accessing dynamic properties using dictionary syntax
            jsonPrompt["93"]["inputs"]["seed"] = seed;
        }

        // get the target node
        if (jsonPrompt.ContainsKey("6"))
        {
            // Accessing dynamic properties using dictionary syntax
            jsonPrompt["6"]["inputs"]["text"] = prompt;
        }

        if (jsonPrompt.ContainsKey("90"))
        {
            // Accessing dynamic properties using dictionary syntax
            jsonPrompt["90"]["inputs"]["width"] = width;
            jsonPrompt["90"]["inputs"]["height"] = height;
        }

        // Create the request object
        var request = new JObject();
        request["prompt"] = jsonPrompt;
        request["client_id"] = _clientId;
        var jsonString = JsonConvert.SerializeObject(request);
        // Send the request to the ComfyUI API
        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(endpoint, httpContent);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var responseContent = JsonSerializer.Deserialize<CreatePromptResponse>(responseString);
        return responseContent.PromptId;
    }

    public async Task<CreateFluxPromptResult> CreateFluxPromptWithSeedAndCustomPrompt(string prompt, int seed)
    {
        var endpoint = $"{_endpoint}/prompt";

        // Load and deserialize JSON using Newtonsoft.Json
        var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "json", "FluxOptimizedPrompt.json");
        var jsonData = await File.ReadAllTextAsync(jsonFilePath);

        // Use JObject to access dynamic properties
        var jsonPrompt = JObject.Parse(jsonData);

        // Initialize the seed
        if (jsonPrompt.ContainsKey("6"))
        {
            // Accessing dynamic properties using dictionary syntax
            jsonPrompt["93"]["inputs"]["seed"] = seed;
        }

        // get the target node
        if (jsonPrompt.ContainsKey("6"))
        {
            // Accessing dynamic properties using dictionary syntax
            jsonPrompt["6"]["inputs"]["text"] = prompt;
        }

        // Create the request object
        var request = new JObject();
        request["prompt"] = jsonPrompt;
        request["client_id"] = _clientId;
        var jsonString = JsonConvert.SerializeObject(request);
        // Send the request to the ComfyUI API
        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(endpoint, httpContent);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var responseContent = JsonSerializer.Deserialize<CreatePromptResponse>(responseString);
        return new CreateFluxPromptResult() { PromptId = responseContent.PromptId, Prompt = request.ToString() };
    }

    public async Task<string> CreateFluxOptimizedAsync(string prompt)
    {
        var endpoint = $"{_endpoint}/prompt";

        // Load and deserialize JSON using Newtonsoft.Json
        var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "json", "FluxOptimizedPrompt.json");
        var jsonData = await File.ReadAllTextAsync(jsonFilePath);

        // Use JObject to access dynamic properties
        var jsonPrompt = JObject.Parse(jsonData);

        // Initialize the seed
        var random = new Random();
        if (jsonPrompt.ContainsKey("6"))
        {
            // Accessing dynamic properties using dictionary syntax
            jsonPrompt["93"]["inputs"]["seed"] = random.Next(1, 1000000);
        }

        // get the target node
        if (jsonPrompt.ContainsKey("6"))
        {
            // Accessing dynamic properties using dictionary syntax
            jsonPrompt["6"]["inputs"]["text"] = prompt;
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
                Console.WriteLine($"Attempt {attempt} failed. Retrying in {retryDelayMs / 1000} seconds...");
                await Task.Delay(retryDelayMs);
            }
            else
            {
                Console.WriteLine($"Max retries ({maxRetries}) reached. Prompt status check failed.");
            }
        }

        return false;
    }

    public async Task<UploadImageResponse> UploadImageToComfyUI(IFormFile file)
    {
        var endpoint = $"{_endpoint}/upload/image";
        var formData = new MultipartFormDataContent();
        var filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
        formData.Add(new StreamContent(file.OpenReadStream()), "image", filename);
        var response = await _httpClient.PostAsync(endpoint, formData);
        response.EnsureSuccessStatusCode();
        var responseObject = await response.Content.ReadFromJsonAsync<UploadImageResponse>();
        return responseObject;
    }

    public string GetUploadImageUrl(string imageName, string subfolder = "", string type = "input")
    {
        var endpoint = $"{_endpoint}/view?subfolder={subfolder}&type={type}&filename={imageName}";
        return endpoint;
    }
}