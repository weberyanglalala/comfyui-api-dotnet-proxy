using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Dotnet8LineRichMenu.Models.ComfyUIApi.Prompt;

public class CreatePromptResponse
{
    [JsonPropertyName("prompt_id")]
    [Required]
    public string PromptId { get; set; }

    [JsonPropertyName("number")]
    [Required]
    public int Number { get; set; }
    
    [JsonPropertyName("node_errors")]
    [JsonIgnore]
    public string NodeErrors { get; set; }
}