using APIRESPONSE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.DTOs;
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
        public async Task<IActionResult> AddRaing(RatingDTO ratingDTO)
        {
            try
            {
                var rating = await _databaseContext.Ratings
                    .FirstOrDefaultAsync(r => r.UserId == ratingDTO.UserId && r.RestaurantId == ratingDTO.RestaurantId);
                if (rating == null)
                {
                    var r = new Rating
                    {
                        UserId = ratingDTO.UserId,
                        RestaurantId = (int)ratingDTO.RestaurantId,
                        Point = rating.Point,
                    };
                    await _databaseContext.Ratings.AddAsync(r);
                    await _databaseContext.SaveChangesAsync();
                }
                else
                {
                    rating.Point = rating.Point;
                    _databaseContext.Ratings.Update(rating);
                    await _databaseContext.SaveChangesAsync();
                }
                var res = await _databaseContext.Restaurants
                    .Include(r => r.Rating)
                    .FirstOrDefaultAsync(r => r.Id == ratingDTO.RestaurantId);
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

        [HttpPost("rating-appetizer")]
        public async Task<IActionResult> AddRaingAppertizer(RatingDTO ratingDTO)
        {
            try
            {
                var rating = await _databaseContext.Ratings
                    .FirstOrDefaultAsync(r => r.UserId == ratingDTO.UserId && r.AppetizerId == ratingDTO.AppetizerId);
                if (rating == null)
                {
                    var r = new Rating
                    {
                        UserId = ratingDTO.UserId,
                        AppetizerId = ratingDTO.AppetizerId,
                        Point = ratingDTO.Point,
                    };
                    await _databaseContext.Ratings.AddAsync(r);
                    await _databaseContext.SaveChangesAsync();
                }
                else
                {
                    rating.Point = ratingDTO.Point;
                    _databaseContext.Ratings.Update(rating);
                    await _databaseContext.SaveChangesAsync();
                }
                var res = await _databaseContext.Appetizers
                    .Include(r => r.Rating)
                    .FirstOrDefaultAsync(r => r.Id == ratingDTO.AppetizerId);
                if (res != null)
                {
                    var totalRatings = res.Rating.Count();

                    var totalPoints = res.Rating.Sum(r => r.Point);
                    var averageRating = totalPoints / totalRatings;
                    res.TotalRating = averageRating;
                    res.CountRatings = totalRatings;
                    _databaseContext.Update(res);
                    await _databaseContext.SaveChangesAsync();
                }
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Successful assessment",
                    Data = ratingDTO
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
        [HttpGet("get-rating/{userId}/{appetizerId}")]
        public async Task<IActionResult> GetRaingAppetizer(int userId, int appetizerId)
        {
            var rating = await _databaseContext.Ratings
                    .FirstOrDefaultAsync(r => r.UserId == userId && r.AppetizerId == appetizerId);
            return Ok(rating);
        }
        [HttpGet]
        public async Task<IActionResult> GetRaing()
        {
            var rating = await _databaseContext.Ratings
                    .ToListAsync();
            return Ok(rating);
        }


        [HttpPost("rating-dish")]
        public async Task<IActionResult> AddRaingDish(RatingDTO ratingDTO)
        {
            try
            {
                var rating = await _databaseContext.Ratings
                    .FirstOrDefaultAsync(r => r.UserId == ratingDTO.UserId && r.DishId == ratingDTO.DishId);
                if (rating == null)
                {
                    var r = new Rating
                    {
                        UserId = ratingDTO.UserId,
                        DishId = ratingDTO.DishId,
                        Point = ratingDTO.Point,
                    };
                    await _databaseContext.Ratings.AddAsync(r);
                    await _databaseContext.SaveChangesAsync();
                }
                else
                {
                    rating.Point = ratingDTO.Point;
                    _databaseContext.Ratings.Update(rating);
                    await _databaseContext.SaveChangesAsync();
                }
                var res = await _databaseContext.Dishes
                    .Include(r => r.Rating)
                    .FirstOrDefaultAsync(r => r.Id == ratingDTO.DishId);
                if (res != null)
                {
                    var totalRatings = res.Rating.Count();

                    var totalPoints = res.Rating.Sum(r => r.Point);
                    var averageRating = totalPoints / totalRatings;
                    res.TotalRating = averageRating;
                    res.CountRatings = totalRatings;
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

        [HttpPost("rating-dessert")]
        public async Task<IActionResult> AddRaingDessert(RatingDTO ratingDTO)
        {
            try
            {
                var rating = await _databaseContext.Ratings
                    .FirstOrDefaultAsync(r => r.UserId == ratingDTO.UserId && r.DessertId == ratingDTO.DessertId);
                if (rating == null)
                {
                    var r = new Rating
                    {
                        UserId = ratingDTO.UserId,
                        DessertId = ratingDTO.DessertId,
                        Point = ratingDTO.Point,
                    };
                    await _databaseContext.Ratings.AddAsync(r);
                    await _databaseContext.SaveChangesAsync();
                }
                else
                {
                    rating.Point = ratingDTO.Point;
                    _databaseContext.Ratings.Update(rating);
                    await _databaseContext.SaveChangesAsync();
                }
                var res = await _databaseContext.Desserts
                    .Include(r => r.Rating)
                    .FirstOrDefaultAsync(r => r.Id == ratingDTO.DessertId);
                if (res != null)
                {
                    var totalRatings = res.Rating.Count();

                    var totalPoints = res.Rating.Sum(r => r.Point);
                    var averageRating = totalPoints / totalRatings;
                    res.TotalRating = averageRating;
                    res.CountRatings = totalRatings;
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
    }
}
