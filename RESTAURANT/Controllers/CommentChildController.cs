using APIRESPONSE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public CommentChildController(ICommentChild commentChild)
        {
            _commentChild = commentChild;
        }

        [HttpPost]
        public async Task<IActionResult> AddCommentChild(CommentChild commentChild)
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
        [HttpPut("{userId}/{commentId}")]
        public async Task<IActionResult> UpdateComment(int userId, int commentId, string comment)
        {
            try
            {
                var comm = await _commentChild.UpdateCommentReply(userId, commentId, comment);
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
