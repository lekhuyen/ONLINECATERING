using APIRESPONSE.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using REDISCLIENT;
using RESTAURANT.API.DTOs;
using RESTAURANT.API.Helpers;
using RESTAURANT.API.Models;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DessertController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DessertController(DatabaseContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;

        }
        [HttpGet]
        public async Task<IActionResult> GetAllDesserts()
        {
            try
            {
                var desserts = await _dbContext.Desserts.ToListAsync();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get desserts successfully",
                    Data= desserts
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
        public async Task<IActionResult> GetDessertById(int id)
        {
            try
            {
                var dessert = await _dbContext.Desserts
                    .Include(x => x.Comments)
                    .Include(x => x.Rating)
                    .ThenInclude(x => x.User)
                    .ThenInclude(x => x.CommentChildren)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (dessert == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Dessert not found",
                        Data = null
                    });
                }

                var dess = new DessertDTO
                {
                    Id = dessert.Id,
                    Name = dessert.DessertName,
                    Image = dessert.DessertImage,
                    Price = dessert.Price,
                    TotalRating = dessert.TotalRating,
                    CountRatings = dessert.CountRatings,
                    Comments = dessert.Comments?.Select(x => new CommentDTO
                    {
                        Id = x.Id,
                        Content = x.Content,
                        User = new UserDTO
                        {
                            Id = x.User.Id,
                            UserName = x.User.UserName
                        },
                        AppetizerId = x.AppetizerId,
                        CommentChildren = x?.CommentChildren?.Select(cc => new CommentChildDTO
                        {
                            Id = cc.Id,
                            Content = cc.Content,
                            UserId = cc.UserId,
                            CommentId = cc.CommentId
                        }).ToList()
                    }).ToList(),
                    Ratings = dessert?.Rating?.Select(x => new RatingDTO
                    {
                        Id = x.Id,
                        Point = x.Point,
                        UserId = x.UserId,
                        User = x.User != null ? new UserDTO
                        {
                            Id = x.User.Id,
                            UserName = x.User.UserName,
                        } : null
                    }).ToList() ?? new List<RatingDTO>(),
                };

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Dessert retrieved successfully",
                    Data = dess
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
        [HttpPost]
        public async Task<IActionResult> AddDessert([FromForm] Dessert dessert, IFormFile formFile)
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
                        dessert.DessertImage = result;
                    }

                    await _dbContext.Desserts.AddAsync(dessert);
                    await _dbContext.SaveChangesAsync();

                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Status = 0,
                        Message = "Create dessert Successfully",
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
        public async Task<IActionResult> UpdateDessert(int id, [FromForm] Dessert updatedDessert, IFormFile? formFile)
        {
            var fileUpload = new FileUpload(_webHostEnvironment);

            try
            {
                // Find the existing dessert in the database
                var existingDessert = await _dbContext.Desserts.FindAsync(id);

                if (existingDessert == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Dessert not found",
                        Data = null
                    });
                }

                if (id != updatedDessert.Id)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Mismatched ID in object and parameter",
                        Data = null
                    });
                }

                // Update the properties of the existing dessert with the new values
                existingDessert.DessertName = updatedDessert.DessertName;
                existingDessert.Price = updatedDessert.Price;
                existingDessert.Quantity = updatedDessert.Quantity;

                // Handle image update
                if (formFile != null)
                {
                    // Delete old image if exists
                    if (!string.IsNullOrEmpty(existingDessert.DessertImage))
                    {
                        fileUpload.DeleteImage(existingDessert.DessertImage);
                    }

                    // Save the new image
                    string newImagePath = await fileUpload.SaveImage("images", formFile);
                    existingDessert.DessertImage = newImagePath;
                }

                // Save changes to the database
                _dbContext.Desserts.Update(existingDessert);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Dessert updated successfully",
                    Data = existingDessert
                });
            }
            catch (Exception ex)
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDessert(int id)
        {
            var fileUpload = new FileUpload(_webHostEnvironment);

            try
            {
                var dessert = await _dbContext.Desserts.FindAsync(id);

                if (dessert == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Dessert not found",
                    });
                }

                
                if (!string.IsNullOrEmpty(dessert.DessertImage))
                {
                    fileUpload.DeleteImage(dessert.DessertImage);
                }

                
                _dbContext.Desserts.Remove(dessert);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Dessert deleted successfully"
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
