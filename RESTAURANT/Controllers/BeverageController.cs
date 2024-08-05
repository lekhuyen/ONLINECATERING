using APIRESPONSE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.DTOs;
using RESTAURANT.API.Helpers;
using RESTAURANT.API.Models;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeverageController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BeverageController(DatabaseContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;

        }

        [HttpGet]
        public async Task<IActionResult> GetAllBeverage()
        {
            try
            {
                var beverages = await _dbContext.Beverages.ToListAsync();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get beverages successfully",
                    Data = beverages
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Internal server error",
                    Data = null
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBeverageById(int id)
        {
            try
            {
                var bever = await _dbContext.Beverages
                    .Include(x => x.Comments)
                    .Include(x => x.Rating)
                    .ThenInclude(x => x.User)
                    .ThenInclude(x => x.CommentChildren)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (bever == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 404,
                        Message = "Beverages not found",
                        Data = null
                    });
                }

                var beverage = new BeverageDTO
                {
                    Id = bever.Id,
                    Name = bever.BeverageName,
                    Image = bever.BeverageImage,
                    Price = bever.Price,
                    TotalRating = bever.TotalRating,
                    CountRatings = bever.CountRatings,
                    Comments = bever?.Comments?.Select(x => new CommentDTO
                    {
                        Id = x.Id,
                        Content = x.Content,
                        User = new UserDTO
                        {
                            Id = x.User.Id,
                            UserName = x.User.UserName
                        },
                        BeverageId = x.BeverageId,
                        CommentChildren = x?.CommentChildren?.Select(cc => new CommentChildDTO
                        {
                            Id = cc.Id,
                            Content = cc.Content,
                            UserId = cc.UserId,
                            CommentId = cc.CommentId
                        }).ToList() ?? new List<CommentChildDTO>()
                    }).ToList(),
                    Ratings = bever.Rating.Select(x => new RatingDTO
                    {
                        Id = x.Id,
                        Point = x.Point,
                        UserId = x.UserId,
                        User = new UserDTO
                        {
                            Id = x.User.Id,
                            UserName = x.User.UserName,
                        }
                    }).ToList(),

                };

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Beverage retrieved successfully",
                    Data = beverage
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Status = 500,
                    Message = "Internal server error",
                    Data = ex.Message
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddBeverage([FromForm] Beverage beverage, IFormFile? formFile)
        {
            var fileUpload = new FileUpload(_webHostEnvironment);

            try
            {
                if (ModelState.IsValid)
                {
                    string result = null;
                    if (formFile != null)
                    {
                        result = await fileUpload.SaveImage("images", formFile);
                        //var imagePath = await FileUpload.SaveImage("images", formFile);
                        beverage.BeverageImage = result;
                    }

                    await _dbContext.Beverages.AddAsync(beverage);
                    await _dbContext.SaveChangesAsync();

                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Status = 0,
                        Message = "Create Beverages Successfully",
                    });
                }
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Empty",
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
        public async Task<IActionResult> UpdateBeverage(int id, [FromForm] Beverage updatedBeverage, IFormFile? formFile)
        {
            var fileUpload = new FileUpload(_webHostEnvironment);

            try
            {
                // Find the existing Beverage in the database
                var existingBeverage = await _dbContext.Beverages.FindAsync(id);

                if (existingBeverage == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Beverage not found",
                        Data = null
                    });
                }

                if (id != existingBeverage.Id)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Mismatched ID in object and parameter",
                        Data = null
                    });
                }

                // Update the properties of the existing beverage with the new values
                existingBeverage.BeverageName = updatedBeverage.BeverageName;
                existingBeverage.Price = updatedBeverage.Price;
                existingBeverage.Quantity = updatedBeverage.Quantity;

                // Handle image update
                if (formFile != null)
                {
                    // Delete old image if exists
                    if (!string.IsNullOrEmpty(existingBeverage.BeverageImage))
                    {
                        fileUpload.DeleteImage(existingBeverage.BeverageImage);
                    }

                    // Save the new image
                    var newImagePath = await fileUpload.SaveImage("images", formFile);
                    existingBeverage.BeverageImage = newImagePath;
                }

                // Save changes to the database
                _dbContext.Beverages.Update(existingBeverage);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Beverage updated successfully",
                    Data = existingBeverage
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Internal server error",
                    Data = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBeverage(int id)
        {
            var fileUpload = new FileUpload(_webHostEnvironment);

            try
            {
                var beverage = await _dbContext.Beverages.FindAsync(id);

                if (beverage == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Beverage not found",
                    });
                }

                if (!string.IsNullOrEmpty(beverage.BeverageImage))
                {
                    fileUpload.DeleteImage(beverage.BeverageImage);
                }

                _dbContext.Beverages.Remove(beverage);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Beverage deleted successfully",
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Internal server error",
                    Data = null
                });
            }
        }
    }
}
