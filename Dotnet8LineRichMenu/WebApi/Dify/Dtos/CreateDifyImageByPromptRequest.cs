using System.Text.Json.Serialization;

namespace Dotnet8LineRichMenu.WebApi.Dify.Dtos;

public class CreateDifyImageByPromptRequest
{
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; }
    [JsonPropertyName("width")]
    public int Width { get; set; }
    [JsonPropertyName("height")]
    public int  Height { get; set; }
    [JsonPropertyName("seed")]
    public int Seed { get; set; }
}