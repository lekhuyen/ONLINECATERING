using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace INFORMATIONAPI.Models
{
    public class About
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }


        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }

        [BsonIgnoreIfNull]
        public string? ImagePath { get; set; }
    }
}
