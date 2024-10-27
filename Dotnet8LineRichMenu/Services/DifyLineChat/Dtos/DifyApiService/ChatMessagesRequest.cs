using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Dotnet8LineRichMenu.Services.DifyLineChat.Dtos.DifyApiService;

public class ChatMessagesRequest
{
    [JsonPropertyName("query")]
    [Required]
    public string Query { get; set; }

    [JsonPropertyName("inputs")]
    [Required]
    public Dictionary<string, object> Inputs { get; set; } = new Dictionary<string, object>();

    [JsonPropertyName("response_mode")]
    [Required]
    public string ResponseMode { get; set; }

    [JsonPropertyName("user")]
    [Required]
    public string User { get; set; }

    [JsonPropertyName("conversation_id")]
    public string ConversationId { get; set; }

    [JsonPropertyName("files")]
    public List<FileDto> Files { get; set; }

    [JsonPropertyName("auto_generate_name")]
    public bool AutoGenerateName { get; set; } = true;
}

public class FileDto
{
    [JsonPropertyName("type")]
    [Required]
    public string Type { get; set; }

    [JsonPropertyName("transfer_method")]
    [Required]
    public string TransferMethod { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("upload_file_id")]
    public string UploadFileId { get; set; }
}