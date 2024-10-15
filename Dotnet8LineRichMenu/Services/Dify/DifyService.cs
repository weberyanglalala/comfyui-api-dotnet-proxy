using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Dotnet8LineRichMenu.Services.Dify.Dtos;

namespace Dotnet8LineRichMenu.Services.Dify;

public class DifyService
{
    private readonly string _createDifyImageApiKey;
    private readonly string _difyGetPromptStatusApiKey;
    private readonly string _difyDifyGetPromptImageUrlApiKey;
    private readonly string _dififyApiUrl;
    private readonly IHttpClientFactory _httpClientFactory;

    public DifyService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _dififyApiUrl = configuration["DifyApiUrl"];
        _createDifyImageApiKey = configuration["DifyCreateImageApiKey"];
        _difyGetPromptStatusApiKey = configuration["DifyGetPromptStatusApiKey"];
        _difyDifyGetPromptImageUrlApiKey = configuration["DifyGetPromptImageUrlApiKey"];
    }

    public async Task<DifyWorkflowResponse> CreateDifyImageByPrompt(DifyWorkflowRequest request)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _createDifyImageApiKey);
        var endpoint = $"{_dififyApiUrl}/workflows/run";
        var jsonContent = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(endpoint, content);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            var runWorkflowResponse = JsonSerializer.Deserialize<DifyWorkflowResponse>(result);
            return runWorkflowResponse;
        }
        else
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error running workflow: {errorResponse}");
        }
    }
}