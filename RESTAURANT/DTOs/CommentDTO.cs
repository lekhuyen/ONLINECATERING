using RESTAURANT.API.Models;

namespace RESTAURANT.API.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public List<CommentChildDTO>? CommentChildren { get; set; }
    }
}
