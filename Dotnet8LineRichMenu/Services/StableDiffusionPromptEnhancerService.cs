using System.Text.Json;
using Dotnet8LineRichMenu.Models.Dtos.Openai;
using OpenAI.Chat;

namespace Dotnet8LineRichMenu.Services;

public class StableDiffusionPromptEnhancerService
{
    private readonly string _apiKey;

    public StableDiffusionPromptEnhancerService(IConfiguration configuration)
    {
        _apiKey = configuration["OpenAiApiKey"];
    }

    public async Task<StableDiffusionResult> CreateEnhancedPrompt(string prompt)
    {
        ChatClient client = new("gpt-4o-mini", _apiKey);

        ChatCompletionOptions options = new()
        {
            ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                name: "stable_diffusion_prompts",
                jsonSchema: BinaryData.FromString("""
                                                  {
                                                    "type": "object",
                                                    "properties": {
                                                      "positivePrompt": { "type": "string"},
                                                      "negativePrompt": { "type": "string" }
                                                    },
                                                    "required": ["positivePrompt", "negativePrompt"],
                                                    "additionalProperties": false
                                                  }
                                                  """),
                strictSchemaEnabled: true)
        };

        List<ChatMessage> messages =
        [
            new SystemChatMessage(
                "Please generate a stable diffusion prompt with positive prompt and negative in English."),
            new UserChatMessage(prompt),
        ];

        ChatCompletion chatCompletion = await client.CompleteChatAsync(messages, options);
        var chatCompletionText = chatCompletion.Content[0].Text;
        var result = JsonSerializer.Deserialize<StableDiffusionResult>(chatCompletionText);
        return result;
    }

    public async Task<string> OptimizePromptToEnglish(string input)
    {
        ChatClient client = new("gpt-4o-mini", _apiKey);
        ChatCompletion completion = await client.CompleteChatAsync(
        [
            new SystemChatMessage("Rephrase user input and translate into english into a fluent sentence."),
            new UserChatMessage(input)
        ]);
        return completion.Content[0].Text;
    }
}