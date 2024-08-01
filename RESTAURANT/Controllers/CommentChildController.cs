using APIRESPONSE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.DTOs;
using RESTAURANT.API.Models;
using RESTAURANT.API.Repositories;
using System.Xml.Linq;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentChildController : ControllerBase
    {
        private readonly ICommentChild _commentChild;
        private readonly DatabaseContext _dbContext;
        public CommentChildController(ICommentChild commentChild, DatabaseContext dbContext)
        {
            _commentChild = commentChild;
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCommentReply()
        {
            var replies = await _dbContext.CommentChildren.ToListAsync();
            return Ok(replies);
        }

        [HttpPost]
        public async Task<IActionResult> AddCommentChild(CommentChildDTO commentChild)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var comm = await _commentChild.AddCommentReply(commentChild);
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Status = 0,
                        Data = comm
                    });
                }
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Error from service",
                    Data = null
                });
            }
        }
        [HttpDelete("{userId}/{commentId}")]
        public async Task<IActionResult> DeleteComment(int userId, int commentId)
        {
            try
            {
                await _commentChild.DeleteCommentReply(userId, commentId);
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Successfully deleted comment"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Error from service",
                    Data = null
                });
            }
        }
        [HttpPut("{userId}/{replyId}")]
        public async Task<IActionResult> UpdateComment(EditCommentReplyDTO commentChildDTO)
        {
            try
            {
                var comm = await _commentChild.UpdateCommentReply(commentChildDTO);
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Successfully updated comment",
                    Data = comm
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Error from service",
                    Data = null
                });
            }
        }
    }
}
