using RESTAURANT.API.Models;

namespace RESTAURANT.API.Repositories
{
    public interface ICommentChild
    {
        Task DeleteCommentReply(int userId, int commentReplyId);
        Task<string> UpdateCommentReply(int userId, int commentReplyId, string commentReply);
        Task<CommentChild> AddCommentReply(CommentChild commentReply);
    }
}
