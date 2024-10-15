using System.Text.Json.Serialization;

namespace Dotnet8LineRichMenu.WebApi.CharacterPhoto.Dtos;

public class CreateCharacterPhotoResponse
{
    [JsonPropertyName("prompt_id")]
    public string PromptId { get; set; }
    [JsonPropertyName("style")]
    public string Style { get; set; }
    [JsonPropertyName("seed")]
    public int Seed { get; set; }
    [JsonPropertyName("count")]
    public int Count { get; set; }
    [JsonPropertyName("width")]
    public int Width { get; set; }
    [JsonPropertyName("height")]
    public int Height { get; set; }
}