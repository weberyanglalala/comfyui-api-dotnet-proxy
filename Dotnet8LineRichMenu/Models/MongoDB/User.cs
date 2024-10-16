using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Dotnet8LineRichMenu.Models.MongoDB
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("passwordHash")]
        public string PasswordHash { get; set; }
        
        [BsonElement("isVerified")]
        public bool IsVerified { get; set; }
    }
}