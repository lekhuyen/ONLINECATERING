using APIRESPONSE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.DTOs;
using RESTAURANT.API.Models;
using RESTAURANT.API.Repositories;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IComment _comment;
        private readonly DatabaseContext _databaseContext;
        public CommentController(IComment comment, DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _comment = comment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComment()
        {
            var comments = await _databaseContext.Comments.ToListAsync();
            return Ok(comments);
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(CommentDTO comment)
        {
            try
            {
                if (ModelState.IsValid)
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
        public async Task<IActionResult> UpdateComment(EditCommentDTO editCommentDTO)
        {
            try
            {
                var comm = await _comment.UpdateComment(editCommentDTO);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCommentStatus(StatusUpdateDTO statusUpdateDTO)
        {
            try
            {
                var comment = await _databaseContext.Comments.FindAsync(statusUpdateDTO.Id);

                if (comment == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Comment not found",
                        Data = null
                    });
                }

                comment.Status = statusUpdateDTO.Status;
                _databaseContext.Update(comment);
                await _databaseContext.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Successfully updated comment status",
                    Data = comment
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
