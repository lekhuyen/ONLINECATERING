namespace RESTAURANT.API.DTOs
{
    public class EditCommentDTO
    {
        public int UserId { get; set; }
        public int CommentId { get; set; }
        public string Comment { get; set; }
    }
}
