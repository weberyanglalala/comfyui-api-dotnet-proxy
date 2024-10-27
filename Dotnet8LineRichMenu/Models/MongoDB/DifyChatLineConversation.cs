using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Dotnet8LineRichMenu.Models.MongoDB;

public class DifyChatLineConversation
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ConversationId { get; set; }
    
    [BsonElement("difyConversationId")]
    public string DifyConversationId { get; set; }

    [BsonElement("createAt")]
    public DateTime CreateAt { get; set; }

    [BsonElement("updateAt")]
    public DateTime UpdateAt { get; set; }

    [BsonElement("status")]
    public int Status { get; set; }

    [BsonElement("userId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; }
}