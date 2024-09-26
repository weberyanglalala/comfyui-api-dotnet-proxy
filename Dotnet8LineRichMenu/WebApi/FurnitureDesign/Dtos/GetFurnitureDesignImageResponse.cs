using System.Text.Json.Serialization;

namespace Dotnet8LineRichMenu.WebApi.FurnitureDesign.Dtos;

public class GetFurnitureDesignImageResponse
{
    [JsonPropertyName("is_success")] public bool IsSuccess { get; set; }
    [JsonPropertyName("public_url")] public string PublicUrl { get; set; }
    [JsonPropertyName("message")] public string Message { get; set; }
}