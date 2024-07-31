using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.DTOs;
using RESTAURANT.API.Models;
using System.ComponentModel.Design;

namespace RESTAURANT.API.Repositories
{
    public class CommentChildRepositories : ICommentChild
    {
        private readonly DatabaseContext _dbContext;
        public CommentChildRepositories(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<CommentChildDTO> AddCommentReply(CommentChildDTO commentReply)
        {
            var commReply = new CommentChild
            {
                UserId = commentReply.UserId,
                CommentId = (int)commentReply.CommentId,
                Content = commentReply.Content,
            };
            await _dbContext.CommentChildren.AddAsync(commReply);
            await _dbContext.SaveChangesAsync();
            return commentReply;
        }

        public async Task DeleteCommentReply(int userId, int commentReplyId)
        {
            var commReply = await _dbContext.CommentChildren
                .FirstOrDefaultAsync(c => c.Id == commentReplyId &&  c.UserId == userId);
            if (commReply != null)
            {
                 _dbContext.CommentChildren.Remove(commReply);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<string> UpdateCommentReply(int userId, int commentReplyId, string commentReply)
        {
            var comm = await _dbContext.CommentChildren
                .FirstOrDefaultAsync(c => c.Id == commentReplyId && c.UserId == userId);
            if (comm != null)
            {
                comm.Content = commentReply;
            }
            _dbContext.Update(comm);
            await _dbContext.SaveChangesAsync();
            return commentReply;
        }
    }
}
