using System.Text.Json.Serialization;

namespace Dotnet8LineRichMenu.WebApi.StyleSwap.Dtos;

public class CreateStyleChangeImageByPromptResponse
{
    [JsonPropertyName("seed")] public int Seed { get; set; }
    [JsonPropertyName("image_name")] public string ImageName { get; set; }
    [JsonPropertyName("style")] public string Style { get; set; }
    [JsonPropertyName("width")] public int Width { get; set; }
    [JsonPropertyName("height")] public int Height { get; set; }
    [JsonPropertyName("prompt_id")] public string PromptId { get; set; }
}