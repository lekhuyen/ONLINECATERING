using MongoDB.Bson.Serialization.Attributes;

namespace USER.API.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; } = "User";
        public string? RefeshToken { get; set; }
        //123
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
    
}
