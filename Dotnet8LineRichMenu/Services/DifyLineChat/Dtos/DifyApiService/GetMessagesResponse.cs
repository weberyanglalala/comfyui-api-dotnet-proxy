using System.Text.Json.Serialization;

namespace Dotnet8LineRichMenu.Services.DifyLineChat.Dtos.DifyApiService;

public class GetMessagesResponse
{
    [JsonPropertyName("limit")]
    public int Limit { get; set; }

    [JsonPropertyName("has_more")]
    public bool HasMore { get; set; }

    [JsonPropertyName("data")]
    public List<MessageDto> Data { get; set; }
}

public class MessageDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("conversation_id")]
    public string ConversationId { get; set; }

    [JsonPropertyName("parent_message_id")]
    public string ParentMessageId { get; set; }

    [JsonPropertyName("inputs")]
    public Dictionary<string, object> Inputs { get; set; }

    [JsonPropertyName("query")]
    public string Query { get; set; }

    [JsonPropertyName("answer")]
    public string Answer { get; set; }

    [JsonPropertyName("message_files")]
    public List<object> MessageFiles { get; set; }

    [JsonPropertyName("feedback")]
    public object Feedback { get; set; }

    [JsonPropertyName("retriever_resources")]
    public List<object> RetrieverResources { get; set; }

    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    [JsonPropertyName("agent_thoughts")]
    public List<object> AgentThoughts { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("error")]
    public object Error { get; set; }
}