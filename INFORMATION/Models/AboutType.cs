using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace INFORMATION.API.Models
{
    public class AboutType
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required(ErrorMessage = "NewsTypeName is required")]
        public string AboutTypeName { get; set; }
    }
}
