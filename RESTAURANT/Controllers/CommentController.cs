using APIRESPONSE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTAURANT.API.Models;
using RESTAURANT.API.Repositories;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IComment _comment;
        public CommentController(IComment comment)
        {
            _comment = comment;
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(Comment comment)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var comm = await _comment.AddComment(comment);
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
                await _comment.DeleteComment(userId, commentId);
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
        [HttpPut("{userId}/{commentId}")]
        public async Task<IActionResult> UpdateComment(int userId,int commentId, string comment)
        {
            try
            {
                var comm = await _comment.UpdateComment(userId, commentId, comment);
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
