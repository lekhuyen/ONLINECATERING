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
        [MinLength(3, ErrorMessage = "Title must contain at least 3 words")]
        public string Title { get; set; }


        [Required(ErrorMessage = "Content is required")]
        [MinLength(10, ErrorMessage = "Content must contain at least 10 words")]
        public string Content { get; set; }

        [BsonIgnoreIfNull]
        public string? ImagePath { get; set; }
    }
}
