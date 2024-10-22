using Dotnet8LineRichMenu.Models.MongoDB;
using Dotnet8LineRichMenu.Models.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Dotnet8LineRichMenu.Services.DifyLineChat;

public class DifyLineChatService
{
    private readonly IMongoCollection<DifyChatLineUser> _lineUsers;
    private readonly IMongoCollection<DifyChatLineConversation> _lineConversations;
    private readonly MongoDbSettings _mongoDbSettings;
    private readonly IConfiguration _configuration;

    public DifyLineChatService(IOptions<MongoDbSettings> mongoDbSettings, IConfiguration configuration,
        IMongoCollection<DifyChatLineConversation> lineConversations)
    {
        _configuration = configuration;
        _lineConversations = lineConversations;
        _mongoDbSettings = mongoDbSettings.Value;

        var client = new MongoClient(_mongoDbSettings.ConnectionString);
        var database = client.GetDatabase(_mongoDbSettings.DatabaseName);
        _lineUsers = database.GetCollection<DifyChatLineUser>("DifyChatLineUsers");
    }

    public async Task<string> GetLineUserIdByDifyUserId(string difyUserId)
    {
        var user = await _lineUsers.Find(lineUser => lineUser.Id == difyUserId)
            .FirstOrDefaultAsync();
        return user?.LineUserId;
    }

    public async Task<DifyChatLineConversation> GetConversationById(string conversationId)
    {
        var conversation = await _lineConversations.Find(conversation => conversation.ConversationId == conversationId)
            .FirstOrDefaultAsync();
        return conversation;
    }
}