using RESTAURANT.API.DTOs;
using RESTAURANT.API.Models;

namespace RESTAURANT.API.Repositories
{
    public interface ICommentChild
    {
        Task DeleteCommentReply(int userId, int commentReplyId);
        Task<string> UpdateCommentReply(EditCommentReplyDTO commentChildDTO);
        Task<CommentChild> AddCommentReply(CommentChildDTO commentReply);
    }
}
