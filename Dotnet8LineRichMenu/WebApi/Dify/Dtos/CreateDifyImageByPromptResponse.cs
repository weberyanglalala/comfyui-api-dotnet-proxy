using System.Text.Json.Serialization;

namespace Dotnet8LineRichMenu.WebApi.Dify.Dtos;

public class CreateDifyImageByPromptResponse
{
    [JsonPropertyName("prompt_id")]
    public string PromptId { get; set; }
    [JsonPropertyName("status")]
    public string Status { get; set; }
    [JsonPropertyName("width")]
    public int Width { get; set; }
    [JsonPropertyName("height")]
    public int Height { get; set; }
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; }
    [JsonPropertyName("seed")]
    public int Seed { get; set; }
}