namespace RESTAURANT.API.DTOs
{
    public class EditCommentReplyDTO
    {
        public int UserId { get; set; }
        public int ReplyId { get; set; }
        public string Content { get; set; }
    }
}
