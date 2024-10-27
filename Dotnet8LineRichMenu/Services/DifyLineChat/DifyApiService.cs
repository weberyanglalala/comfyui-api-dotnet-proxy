using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Dotnet8LineRichMenu.Services.DifyLineChat.Dtos.DifyApiService;

namespace Dotnet8LineRichMenu.Services.DifyLineChat;

public class DifyApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _apiUrl;

    public DifyApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _apiKey = configuration["HikaruDifyChatApiKey"];
        _apiUrl = configuration["HikaruDifyApiUrl"];
        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
    }

    public async Task<ChatMessagesResponse> SendChatMessageAsync(ChatMessagesRequest request)
    {
        var jsonRequest = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_apiUrl}/chat-messages", content);
        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error from SendChatMessageAsync DIFY API: {errorResponse}");
        }

        var resultString = new StringBuilder();
        var conversationId = string.Empty;
        // await foreach (var chunk in response.Content.ReadFromJsonAsAsyncEnumerable<ChunkChatCompletionResponse>())
        // {
        //     Console.WriteLine(chunk.Answer);
        //     if (chunk is { Event: "message", Answer: not null })
        //     {
        //         Console.WriteLine(chunk.Answer);
        //         resultString.Append(chunk.Answer);
        //         if (!string.IsNullOrWhiteSpace(chunk.ConversationId))
        //         {
        //             conversationId = chunk.ConversationId;
        //         }
        //     }
        // }
        string difyConversationId = string.Empty;
        if (response.IsSuccessStatusCode)
        {
            var stream = await response.Content.ReadAsStreamAsync();
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    if (!string.IsNullOrWhiteSpace(line) && line.StartsWith("data:"))
                    {
                        var json = line.Substring(5); // Remove "data:" prefix
                        var chunk = JsonSerializer.Deserialize<ChunkChatCompletionResponse>(json);
                        resultString.Append(chunk.Answer);
                        if (difyConversationId == string.Empty)
                        {
                            difyConversationId = chunk.ConversationId;
                        }
                    }
                }
            }

            return new ChatMessagesResponse()
            {
                ConversationId = conversationId,
                Message = resultString.ToString(),
                DifyConversationId = difyConversationId
            };
        }
        else
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error from API: {errorResponse}");
        }
    }

    public async Task<GetMessagesResponse> GetHistoryMessageAsync(GetMessagesRequest getMessageRequest)
    {
        var response = await _httpClient.GetAsync(
            $"{_apiUrl}/messages?user={getMessageRequest.User}&conversation_id={getMessageRequest.ConversationId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<GetMessagesResponse>();
    }
}