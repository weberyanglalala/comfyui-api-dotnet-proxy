using System.Text.Json.Serialization;

namespace Dotnet8LineRichMenu.WebApi.CharacterPhoto.Dtos;

public class CreateCharacterPhotoRequest
{
    [JsonPropertyName("image_path")]
    public string ImagePath { get; set; }
    [JsonPropertyName("style")]
    public string Style { get; set; }
    [JsonPropertyName("seed")]
    public int Seed { get; set; }
    [JsonPropertyName("count")]
    public int Count { get; set; } = 4;

    [JsonPropertyName("width")]
    public int Width { get; set; } = 864;
    [JsonPropertyName("height")]
    public int Height { get; set; } = 1536;
}