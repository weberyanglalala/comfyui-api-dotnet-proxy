using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Dotnet8LineRichMenu.Services.DifyLineChat.Dtos.DifyApiService;

public class GetMessagesRequest
{
    [JsonPropertyName("user")]
    [Required]
    public string User { get; set; }

    [JsonPropertyName("conversation_id")]
    [Required]
    public string ConversationId { get; set; }
}