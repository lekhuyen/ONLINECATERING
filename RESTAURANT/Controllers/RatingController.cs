using APIRESPONSE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.Models;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        public RatingController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        [HttpPost("{userId}/{restaurantId}")]
        public async Task<IActionResult> AddRaing(int userId, int restaurantId, int point)
        {
            try
            {
                var rating = await _databaseContext.Ratings
                    .FirstOrDefaultAsync(r => r.UserId == userId && r.RestaurantId == restaurantId);
                if (rating == null)
                {
                    var r = new Rating
                    {
                        UserId = userId,
                        RestaurantId = restaurantId,    
                        Point = point,
                    };
                    await _databaseContext.Ratings.AddAsync(r);
                    await _databaseContext.SaveChangesAsync();
                }
                else
                {
                    rating.Point = point;
                    _databaseContext.Ratings.Update(rating);
                    await _databaseContext.SaveChangesAsync();
                }
                var res = await _databaseContext.Restaurants
                    .Include(r => r.Rating)
                    .FirstOrDefaultAsync(r => r.Id == restaurantId);
                if (res != null)
                {
                    var totalRatings = res.Rating.Count();
                    
                    var totalPoints = res.Rating.Sum(r => r.Point);
                    var averageRating = totalPoints / totalRatings;
                    res.TotalRating = averageRating;
                    _databaseContext.Update(res);
                    await _databaseContext.SaveChangesAsync();
                }
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Successful assessment",
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
        [HttpGet("{userId}/{restaurantId}")]
        public async Task<IActionResult> GetRaing(int userId, int restaurantId)
        {
            var rating = await _databaseContext.Ratings
                    .FirstOrDefaultAsync(r => r.UserId == userId && r.RestaurantId == restaurantId);
            return Ok(rating);
        }
    }
}
