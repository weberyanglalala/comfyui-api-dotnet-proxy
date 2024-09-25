using System.Text.Json.Serialization;

namespace Dotnet8LineRichMenu.Models.ComfyUIApi.Prompt;

public class CreateStyleChangeImageByPromptRequest
{
    [JsonPropertyName("seed")] public int Seed { get; set; }
    [JsonPropertyName("image_name")] public string ImageName { get; set; }
    [JsonPropertyName("style")] public string Style { get; set; }
    [JsonPropertyName("width")] public int Width { get; set; }
    [JsonPropertyName("height")] public int Height { get; set; }
}