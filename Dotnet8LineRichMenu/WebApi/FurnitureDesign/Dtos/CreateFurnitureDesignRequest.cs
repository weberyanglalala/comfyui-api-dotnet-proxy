using System.Text.Json.Serialization;

namespace Dotnet8LineRichMenu.WebApi.FurnitureDesign.Dtos;

public class CreateFurnitureDesignRequest
{
    [JsonPropertyName("image_path")]
    public string ImagePath { get; set; }
    [JsonPropertyName("style")]
    public string Style { get; set; }
    [JsonPropertyName("seed")]
    public int Seed { get; set; }
}