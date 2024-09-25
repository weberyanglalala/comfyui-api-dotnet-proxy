using System.Text.Json.Serialization;

namespace Dotnet8LineRichMenu.WebApi.FluxOptimize.Dtos;

public class CreateBySeedPromptSizeResponse
{
    [JsonPropertyName("prompt")] public string Prompt { get; set; }
    [JsonPropertyName("prompt_id")] public string PromptId { get; set; }
    [JsonPropertyName("seed")] public int Seed { get; set; }
    [JsonPropertyName("width")] public int Width { get; set; }
    [JsonPropertyName("height")] public int Height { get; set; }
}