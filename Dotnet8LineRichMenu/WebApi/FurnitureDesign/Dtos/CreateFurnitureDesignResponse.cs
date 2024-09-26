using System.Text.Json.Serialization;

namespace Dotnet8LineRichMenu.WebApi.FurnitureDesign.Dtos;

public class CreateFurnitureDesignResponse
{
    [JsonPropertyName("prompt_id")]
    public string PromptId { get; set; }
    [JsonPropertyName("style")]
    public string Style { get; set; }
    [JsonPropertyName("seed")]
    public int Seed { get; set; }
}