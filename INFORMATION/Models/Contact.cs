using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace INFORMATIONAPI.Models
{
    public class Contact
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required(ErrorMessage = "FullName is required")]
        [MinLength(3, ErrorMessage = "FullName must contain at least 3 words")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [MinLength(5, ErrorMessage = "Email must contain at least 5 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [MinLength(10, ErrorMessage = "Phone must contain at least 10 characters")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Content is required")]
        [MinLength(10, ErrorMessage = "Content must contain at least 10 words")]
        public string Content { get; set; }

        [DefaultValue(false)]
        public bool IsAdminResponse { get; set; } 

        [BsonIgnoreIfNull]
        [DefaultValue("")]
        public string? ResponseMessage { get; set; } 

        public DateTime? ResponseDate { get; set; } 
    }
}
