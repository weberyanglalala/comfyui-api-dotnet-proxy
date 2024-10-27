using Dotnet8LineRichMenu.Models.MongoDB;
using Dotnet8LineRichMenu.Services.DifyLineChat.Dtos.DifyApiService;

namespace Dotnet8LineRichMenu.Services.DifyLineChat;

public class DifyLineChatService
{
    private readonly LineChatMongoService _lineChatMongoService;
    private readonly DifyApiService _difyApiService;

    public DifyLineChatService(LineChatMongoService lineChatMongoService, DifyApiService difyApiService)
    {
        _lineChatMongoService = lineChatMongoService;
        _difyApiService = difyApiService;
    }

    public async Task<string> HandleLineChat(string lineUserId, string displayName, string message)
    {
        // Check if Line User exists
        var user = await GetOrCreateUser(lineUserId, displayName);

        // Check if Conversation exists for the user
        var conversation = await _lineChatMongoService.GetCurrentConversationByUserId(user.Id);

        // If no conversation exists, create one
        var response = await SendMessageAsync(user.Id, message, conversation?.DifyConversationId);
        if (conversation == null)
        {
            await _lineChatMongoService.CreateConversationByLineUserId(lineUserId, response.DifyConversationId);
        }

        return response.Message;
    }

    private async Task<DifyChatLineUser> GetOrCreateUser(string lineUserId, string displayName)
    {
        var user = await _lineChatMongoService.GetUserByLineUserId(lineUserId);
        if (user == null)
        {
            user = await _lineChatMongoService.CreateLineUser(lineUserId, displayName);
        }

        return user;
    }

    private async Task<ChatMessagesResponse> SendMessageAsync(string userId, string message, string conversationId)
    {
        var response = await _difyApiService.SendChatMessageAsync(new ChatMessagesRequest
        {
            User = userId,
            Query = message,
            ResponseMode = "streaming",
            ConversationId = conversationId ?? string.Empty,
            Inputs = { }
        });
        return response;
    }
}