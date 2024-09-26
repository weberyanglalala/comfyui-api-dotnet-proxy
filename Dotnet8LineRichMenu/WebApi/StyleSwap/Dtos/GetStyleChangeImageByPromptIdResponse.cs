using System.Text.Json.Serialization;

namespace Dotnet8LineRichMenu.WebApi.StyleSwap.Dtos;

public class GetStyleChangeImageByPromptIdResponse
{
    [JsonPropertyName("is_success")] public bool IsSuccess { get; set; }
    [JsonPropertyName("public_url")] public string PublicUrl { get; set; }
    [JsonPropertyName("message")] public string Message { get; set; }
}