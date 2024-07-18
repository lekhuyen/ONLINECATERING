using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace INFORMATIONAPI.Models
{
    public class News
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

        [BsonRepresentation(BsonType.ObjectId)]
        [Required(ErrorMessage = "NewsTypeId is required")]
        public string NewsTypeId { get; set; }

        [BsonIgnoreIfNull]
        public List<string>? ImagePaths { get; set; } = new List<string>();
    }
}
