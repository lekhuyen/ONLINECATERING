using APIRESPONSE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.Helpers;
using RESTAURANT.API.Models;
using RESTAURANT.API.Repositories;
using static System.Net.Mime.MediaTypeNames;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantImagesController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RestaurantImagesController(DatabaseContext databaseContext, IWebHostEnvironment webHostEnvironment)
        {
            _databaseContext = databaseContext;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpPost]
        public async Task<IActionResult> AddImages([FromForm]  int restaurantId, List<IFormFile> formFiles)
        {
            var fileUpload = new FileUpload(_webHostEnvironment);

            try
            {
                if(formFiles.Count()  == 0)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Empty",
                    });
                }
                foreach (var item in formFiles)
                {
                    var imagePath = await fileUpload.SaveImage("images", item);
                    var restaurantImages = new RestaurantImages
                    {
                        RestaurantId = restaurantId,
                        ImagesUrl = imagePath,
                    };
                    await _databaseContext.RestaurantImages.AddAsync(restaurantImages);
                    await _databaseContext.SaveChangesAsync();
                }
                return Created("success", new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Add images Successfully",
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
        [HttpGet("{imagesId}")]
        public async Task<IActionResult> GetImageById(int imagesId)
        {
            try
            {
                var image = await _databaseContext.RestaurantImages.FirstOrDefaultAsync(r => r.Id == imagesId);
                if(image != null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Status = 0,
                        Data = image
                    });
                }
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Not found",
                    Data = null
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
        public async Task<IActionResult> DeleteImage(int id)
        {
            var fileUpload = new FileUpload(_webHostEnvironment);

            try
            {
                var image = await _databaseContext.RestaurantImages.FirstOrDefaultAsync(r => r.Id == id);
                if (image == null)
                {
                    return NotFound();
                }
                fileUpload.DeleteImage(image.ImagesUrl);
                _databaseContext.Remove(image);
                await _databaseContext.SaveChangesAsync();
                return Ok(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Deleted",
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Error from service",
                });
            }
        }
    }
}
