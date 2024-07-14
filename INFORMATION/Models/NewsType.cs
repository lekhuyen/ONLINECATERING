using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace INFORMATIONAPI.Models
{
    public class NewsType
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required(ErrorMessage = "NewsTypeName is required")]
        public string NewsTypeName { get; set; }
    }
}
