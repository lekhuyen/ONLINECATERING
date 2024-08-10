using RESTAURANT.API.Models;

namespace RESTAURANT.API.DTOs
{
    public class CommentChildDTO
    {
        public int? Id { get; set; }
        public int? CommentId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public UserDTO? User { get; set; }
    }
}
