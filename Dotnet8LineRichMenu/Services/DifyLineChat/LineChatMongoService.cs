using Dotnet8LineRichMenu.Models.MongoDB;
using Dotnet8LineRichMenu.Models.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Dotnet8LineRichMenu.Services.DifyLineChat;

public class LineChatMongoService
{
    private readonly IMongoCollection<DifyChatLineUser> _lineUsers;
    private readonly IMongoCollection<DifyChatLineConversation> _lineConversations;

    public LineChatMongoService(IOptions<MongoDbSettings> mongoDbSettings)
    {
        var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
        var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _lineUsers = database.GetCollection<DifyChatLineUser>("DifyChatLineUsers");
        _lineConversations = database.GetCollection<DifyChatLineConversation>("DifyChatLineConversations");
    }

    public async Task<DifyChatLineUser> GetUserByLineUserId(string lineUserId)
    {
        var user = await _lineUsers.Find(lineUser => lineUser.LineUserId == lineUserId)
            .FirstOrDefaultAsync();
        return user;
    }

    public async Task<DifyChatLineUser> GetUserById(string id)
    {
        var user = await _lineUsers.Find(lineUser => lineUser.Id == id)
            .FirstOrDefaultAsync();
        return user;
    }

    public async Task<DifyChatLineUser> CreateLineUser(string lineUserId, string displayName)
    {
        var user = new DifyChatLineUser
        {
            LineUserId = lineUserId,
            DisplayName = displayName,
            CreateAt = DateTime.Now,
            UpdateAt = DateTime.Now
        };

        await _lineUsers.InsertOneAsync(user);
        return user;
    }


    public async Task<DifyChatLineConversation> GetConversationById(string conversationId)
    {
        var conversation = await _lineConversations.Find(conversation => conversation.ConversationId == conversationId)
            .FirstOrDefaultAsync();
        return conversation;
    }

    public async Task<DifyChatLineConversation> GetCurrentConversationIdByLineUserId(string lineUserId)
    {
        var user = await GetUserByLineUserId(lineUserId);
        if (user is null)
        {
            throw new Exception("Line User Id not found.");
        }

        var conversation = await _lineConversations.Find(conversation => conversation.UserId == user.Id)
            .FirstOrDefaultAsync();
        return conversation;
    }

    public async Task<DifyChatLineConversation> GetCurrentConversationByUserId(string userId)
    {
        var user = await GetUserById(userId);
        if (user is null)
        {
            throw new Exception("Line User Id not found.");
        }

        var conversation = await _lineConversations.Find(conversation => conversation.UserId == user.Id)
            .FirstOrDefaultAsync();
        return conversation;
    }

    public async Task<DifyChatLineConversation> CreateConversationByLineUserId(string lineUserId, string difyConversationId)
    {
        var user = await GetUserByLineUserId(lineUserId);
        if (user is null)
        {
            throw new Exception("Line User Id not found.");
        }

        var conversation = new DifyChatLineConversation
        {
            UserId = user.Id,
            CreateAt = DateTime.Now,
            UpdateAt = DateTime.Now,
            DifyConversationId = difyConversationId,
        };

        await _lineConversations.InsertOneAsync(conversation);
        return conversation;
    }
}