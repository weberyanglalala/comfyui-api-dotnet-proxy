using System.Text.Json.Serialization;

namespace Dotnet8LineRichMenu.Models.Dtos.ComfyUI;

public class CreateBySeedAndPromptAndSizeRequest
{
    [JsonPropertyName("prompt")] public string Prompt { get; set; }
    [JsonPropertyName("seed")] public int Seed { get; set; }
    [JsonPropertyName("width")] public int Width { get; set; }
    [JsonPropertyName("height")] public int Height { get; set; }
}