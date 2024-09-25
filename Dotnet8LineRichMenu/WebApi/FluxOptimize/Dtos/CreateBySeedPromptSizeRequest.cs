using System.Text.Json.Serialization;

namespace Dotnet8LineRichMenu.WebApi.FluxOptimize.Dtos;

public class CreateBySeedPromptSizeRequest
{
    [JsonPropertyName("prompt")] public string Prompt { get; set; }
    [JsonPropertyName("seed")] public int Seed { get; set; }
    [JsonPropertyName("width")] public int Width { get; set; }
    [JsonPropertyName("height")] public int Height { get; set; }
}