using System.Text.Json.Serialization;

namespace Dotnet8LineRichMenu.Services.DifyLineChat.Dtos.DifyApiService;

public class ChunkChatCompletionResponse
{
    [JsonPropertyName("event")]
    public string Event { get; set; }

    [JsonPropertyName("task_id")]
    public string TaskId { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("answer")]
    public string Answer { get; set; }

    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    [JsonPropertyName("message_id")]
    public string MessageId { get; set; }

    [JsonPropertyName("conversation_id")]
    public string ConversationId { get; set; }

    [JsonPropertyName("audio")]
    public string Audio { get; set; }

    [JsonPropertyName("thought")]
    public string Thought { get; set; }

    [JsonPropertyName("observation")]
    public string Observation { get; set; }

    [JsonPropertyName("tool")]
    public string Tool { get; set; }

    [JsonPropertyName("tool_input")]
    public Dictionary<string, object> ToolInput { get; set; }

    [JsonPropertyName("file_id")]
    public string FileId { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("belongs_to")]
    public string BelongsTo { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("metadata")]
    public object Metadata { get; set; }

    [JsonPropertyName("usage")]
    public Usage Usage { get; set; }

    [JsonPropertyName("retriever_resources")]
    public List<RetrieverResource> RetrieverResources { get; set; }

    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
}

public class Usage
{
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; set; }

    [JsonPropertyName("completion_tokens")]
    public int CompletionTokens { get; set; }

    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; set; }
}

public class RetrieverResource
{
    [JsonPropertyName("source")]
    public string Source { get; set; }

    [JsonPropertyName("citation")]
    public string Citation { get; set; }
}
