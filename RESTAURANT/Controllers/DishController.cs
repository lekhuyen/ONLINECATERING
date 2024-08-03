using APIRESPONSE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.DTOs;
using RESTAURANT.API.Helpers;
using RESTAURANT.API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {

        private readonly DatabaseContext _dbContext; // Replace DatabaseContext with your DbContext
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DishController(DatabaseContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;

        }

        // GET api/dish
        [HttpGet]
        public async Task<IActionResult> GetAllDishes()
        {
            try
            {
                var dishes = await _dbContext.Dishes
                    .Include(d => d.ComboDishes)
                    .ToListAsync();

                var dishDTOs = dishes.Select(dish => new DishDTO
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Price = dish.Price,

                    Status = dish.Status,
                    Image = dish.ImagePath,
                }).ToList();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get dishes successfully",
                    Data = dishDTOs
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Internal server error",
                    Data = e.Message
                });
            }
        }


        // GET api/dish/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDishById(int id)
        {
            try
            {
                var dish = await _dbContext.Dishes
                    .Include(x => x.Comments)
                    .ThenInclude(x => x.User)
                    .ThenInclude(x => x.CommentChildren)
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (dish == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Dish not found",
                    });
                }

                var dishDTO = new DishDTO
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Price = dish.Price,
                    Status = dish.Status,
                    Image = dish.ImagePath,

                    Comments = dish?.Comments?.Select(x => new CommentDTO
                    {
                        Id = x.Id,
                        Content = x.Content,
                        User = new UserDTO
                        {
                            Id = x.User.Id,
                            UserName = x.User.UserName
                        },
                        CommentChildren = x?.CommentChildren?.Select(cc => new CommentChildDTO
                        {
                            Id = cc.Id,
                            Content = cc.Content,
                            UserId = cc.UserId,
                            CommentId = cc.CommentId
                        }).ToList()
                    }).ToList()
                };

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get dish successfully",
                    Data = dishDTO
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Internal server error",
                    Data = e.Message
                });
            }
        }


        // POST api/dish
        [HttpPost]
        public async Task<IActionResult> CreateDish([FromForm] DishDTO dishDTO, IFormFile formFile)
        {
            var fileUpload = new FileUpload(_webHostEnvironment);
            try
            {
                // Save image if exists
                string result = null;
                if (formFile != null)
                {
                    result = await fileUpload.SaveImage("images", formFile);
                }

                // Map DTO to entity
                var newDish = new Dish
                {
                    Name = dishDTO.Name,
                    Price = dishDTO.Price,

                    Status = (bool)dishDTO.Status,
                    ImagePath = result
                };

                // Add to DbContext
                await _dbContext.Dishes.AddAsync(newDish);
                await _dbContext.SaveChangesAsync();

                return Created("success", new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Add Dish Successfully",
                    Data = newDish
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Internal server error",
                    Data = e.Message
                });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDish(int id, [FromForm] DishDTO dishDTO, IFormFile? formFile)
        {
            var fileUpload = new FileUpload(_webHostEnvironment);

            try
            {
                var existingDish = await _dbContext.Dishes.FindAsync(id);

                if (existingDish == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Dish not found",
                    });
                }

                if (id != dishDTO.Id)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Mismatched ID in about object and parameter",
                        Data = null
                    });
                }

                // Update properties
                existingDish.Name = dishDTO.Name;
                existingDish.Price = dishDTO.Price;

                existingDish.Status = (bool)dishDTO.Status;

                // Handle image update
                if (formFile != null)
                {
                    // Delete old image if it exists
                    if (!string.IsNullOrEmpty(existingDish.ImagePath))
                    {
                        fileUpload.DeleteImage(existingDish.ImagePath);
                    }

                    // Save new image and update ImagePath
                    existingDish.ImagePath = await fileUpload.SaveImage("Images", formFile);
                }

                // Update entity in DbContext
                _dbContext.Dishes.Update(existingDish);
                await _dbContext.SaveChangesAsync();

                var updatedDishDTO = new DishDTO
                {
                    Id = existingDish.Id,
                    Name = existingDish.Name,
                    Price = existingDish.Price,

                    Status = existingDish.Status,
                    Image = existingDish.ImagePath,
                };

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Dish updated successfully",
                    Data = updatedDishDTO
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Internal server error",
                    Data = e.Message
                });
            }
        }


        // DELETE api/dish/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            var fileUpload = new FileUpload(_webHostEnvironment);
            try
            {
                var dishToDelete = await _dbContext.Dishes.FindAsync(id);

                if (dishToDelete == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Dish not found",
                    });
                }

                // Delete associated image if exists
                if (!string.IsNullOrEmpty(dishToDelete.ImagePath))
                {
                    fileUpload.DeleteImage(dishToDelete.ImagePath);
                }

                // Remove from DbContext and save changes
                _dbContext.Dishes.Remove(dishToDelete);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Dish deleted successfully"
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Internal server error",
                    Data = e.Message
                });
            }
        }
    }
}