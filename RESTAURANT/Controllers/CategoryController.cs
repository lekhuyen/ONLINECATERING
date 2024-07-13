using APIRESPONSE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTAURANT.API.Models;
using RESTAURANT.API.Repositories;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategory _icategory;
        public CategoryController(ICategory icategory)
        {
            _icategory = icategory;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categories = await _icategory.GetCategoryAsync();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get categories Successfully",
                    Data = categories
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
        [HttpPost]
        public async Task<IActionResult> AddRestaurant(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _icategory.AddCategoryAsync(category);
                    return Created("success", new ApiResponse
                    {
                        Success = true,
                        Status = 0,
                        Message = "Add category Successfully",
                        Data = res
                    });
                }
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Create category failed"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Create category failed"
                });
            }
        }
    }
}
