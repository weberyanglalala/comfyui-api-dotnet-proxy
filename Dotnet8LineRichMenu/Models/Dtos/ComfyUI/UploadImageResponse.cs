using System.Text.Json.Serialization;

namespace Dotnet8LineRichMenu.Models.Dtos.ComfyUI;

public class UploadImageResponse
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("subfolder")]
    public string Subfolder { get; set; }
    [JsonPropertyName("type")]
    public string Type { get; set; }
}