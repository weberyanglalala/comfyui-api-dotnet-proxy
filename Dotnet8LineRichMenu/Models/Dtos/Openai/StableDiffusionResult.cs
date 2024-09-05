using System.Text.Json.Serialization;

namespace Dotnet8LineRichMenu.Models.Dtos.Openai;

public class StableDiffusionResult
{
    [JsonPropertyName("positivePrompt")]
    public string PositivePrompt { get; set; }
    [JsonPropertyName("negativePrompt")]
    public string NegativePrompt { get; set; }
}