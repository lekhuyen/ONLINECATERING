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
        public async Task<CommentDTO> AddComment(CommentDTO comment)
        {
            var comm = new Comment
            {
                Id = comment.Id,
                UserId = comment.UserId,
                AppetizerId = comment.AppetizerId,
                Content = comment.Content
            };
            await _dbContext.AddAsync(comm);
            await _dbContext.SaveChangesAsync();
            return comment;
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

        

        public async Task<string> UpdateComment(int userId, int commentId, string comment)
        {
            var comm = await _dbContext.Comments
                .FirstOrDefaultAsync(c => c.Id == commentId && c.UserId == userId);
            if(comm != null)
            {
                comm.Content = comment;
            }
            _dbContext.Update(comm);
            await _dbContext.SaveChangesAsync();
            return comment;
        }
    }
}
