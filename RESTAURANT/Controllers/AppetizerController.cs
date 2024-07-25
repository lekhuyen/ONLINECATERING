using APIRESPONSE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.Models;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppetizerController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public AppetizerController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAppetizer()
        {
            try
            {
                var appetizers = await _dbContext.Appetizers.ToListAsync();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get appetizers successfully",
                    Data = appetizers
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
        [HttpPost]
        public async Task<IActionResult> AddAppetizer([FromForm] Appetizer appetizer, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (formFile != null)
                    {
                        var imagePath = await FileUpload.SaveImage("images", formFile);
                        appetizer.AppetizerImage = imagePath;
                    }

                    await _dbContext.Appetizers.AddAsync(appetizer);
                    await _dbContext.SaveChangesAsync();

                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Status = 0,
                        Message = "Create appetizer Successfully",
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppetizer(int id)
        {
            try
            {
                var appetizer = await _dbContext.Appetizers.FindAsync(id);

                if (appetizer == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Appetizer not found",
                    });
                }


                if (!string.IsNullOrEmpty(appetizer.AppetizerImage))
                {
                    FileUpload.DeleteImage(appetizer.AppetizerImage);
                }


                _dbContext.Appetizers.Remove(appetizer);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Appetizer deleted successfully"
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
