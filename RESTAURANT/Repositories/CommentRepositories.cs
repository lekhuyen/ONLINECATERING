using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.DTOs;
using RESTAURANT.API.Models;

namespace RESTAURANT.API.Repositories
{
    public class CommentRepositories : IComment
    {
        private readonly DatabaseContext _dbContext;
        public CommentRepositories(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Comment> AddComment(CommentDTO comment)
        {
            var comm = new Comment
            {
                UserId = comment.UserId,
                AppetizerId = comment.AppetizerId,
                Content = comment.Content,
                DessertId = comment.DessertId,
                DishId = comment.DishId,
            };
            await _dbContext.AddAsync(comm);
            await _dbContext.SaveChangesAsync();
            return comm;
        }

        public async Task DeleteComment(int userId, int commentId)
        {
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.UserId == userId && c.Id == commentId);
            if(comment != null)
            {
                 _dbContext.Comments.Remove(comment);
                await _dbContext.SaveChangesAsync();
            }
        }

        

        public async Task<string> UpdateComment(EditCommentDTO editCommentDTO)
        {
            var comm = await _dbContext.Comments
                .FirstOrDefaultAsync(c => c.Id == editCommentDTO.CommentId && c.UserId == editCommentDTO.UserId);
            if(comm != null)
            {
               comm.Content = editCommentDTO.Comment;
            }
            _dbContext?.Update(comm);
            await _dbContext.SaveChangesAsync();
            return editCommentDTO.Comment;
        }
    }
}
