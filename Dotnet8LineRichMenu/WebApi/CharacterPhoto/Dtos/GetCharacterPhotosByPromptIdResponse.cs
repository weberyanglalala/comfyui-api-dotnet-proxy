using System.Text.Json.Serialization;

namespace Dotnet8LineRichMenu.WebApi.CharacterPhoto.Dtos;

public class GetCharacterPhotosByPromptIdResponse
{
    [JsonPropertyName("public_urls")]
    public List<string> PublicUrls { get; set; }
}