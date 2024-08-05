using APIRESPONSE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USER.API.Models;

namespace USER.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        public GradeController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        [HttpGet("{userId}/{restaurantId}")]
        public async Task<IActionResult> GetPonts(int userId, int restaurantId)
        {
            try
            {
                var point = await _databaseContext.Grades.FirstOrDefaultAsync(u => u.UserId == userId && u.RestaurantId == restaurantId);
                if(point != null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Status = 0,
                        Message = "Get point Successfully",
                        Data = point
                    });
                }
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Point equal zero",
                });
            }
            catch(Exception ex)
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
        [HttpPost("{userId}/{restaurantId}")]
        public async Task<IActionResult> AddPonts(int userId, int restaurantId, int grade)
        {
            try
            {
                var point = await _databaseContext.Grades.FirstOrDefaultAsync(u => u.UserId == userId && u.RestaurantId == restaurantId);

                if( point != null)
                {
                    point.Point += grade;
                     _databaseContext.Update(point);
                    await _databaseContext.SaveChangesAsync();
                }
                else
                {
                    var newPoint = new Grade
                    {
                        UserId = userId,
                        RestaurantId = restaurantId,
                        Point = grade,
                    };
                    await _databaseContext.Grades.AddAsync(newPoint);
                    await _databaseContext.SaveChangesAsync();

                }
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Add grade Successfully",
                });
            }
            catch(Exception ex)
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
        [HttpGet("chat")]
        public async Task<IActionResult> GetChat()
        {
            var chats = await _databaseContext.Messages.ToListAsync();
            return Ok(chats);
        }
    }
}
