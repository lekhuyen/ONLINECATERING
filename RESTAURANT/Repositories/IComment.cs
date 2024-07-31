using RESTAURANT.API.DTOs;
using RESTAURANT.API.Models;

namespace RESTAURANT.API.Repositories
{
    public interface IComment
    {
        Task DeleteComment(int userId, int commentId);
        Task<string> UpdateComment(int userId, int commentId, string comment);
        Task<CommentDTO> AddComment(CommentDTO comment);
    }
}
