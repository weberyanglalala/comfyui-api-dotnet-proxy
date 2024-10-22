using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Dotnet8LineRichMenu.Models.MongoDB;

public class DifyChatLineUser
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("lineUserId")]
    public string LineUserId { get; set; }

    [BsonElement("displayName")]
    public string DisplayName { get; set; }

    [BsonElement("createAt")]
    public DateTime CreateAt { get; set; }

    [BsonElement("updateAt")]
    public DateTime UpdateAt { get; set; }
}