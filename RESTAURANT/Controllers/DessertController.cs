using APIRESPONSE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using REDISCLIENT;
using RESTAURANT.API.DTOs;
using RESTAURANT.API.Models;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DessertController : ControllerBase
    {
        private readonly DatabaseContext _dbContext; 

        public DessertController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
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
        [HttpPost]
        public async Task<IActionResult> AddDessert([FromForm] Dessert dessert,IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (formFile != null)
                    {
                        var imagePath = await FileUpload.SaveImage("images", formFile);
                        dessert.DessertImage = imagePath;
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDessert(int id)
        {
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
                    FileUpload.DeleteImage(dessert.DessertImage);
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
