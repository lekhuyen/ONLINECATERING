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

        public DishController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
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
                    ImagePath = dish.ImagePath,

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
                var dish = await _dbContext.Dishes.FirstOrDefaultAsync(d => d.Id == id);

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
                    ImagePath = dish.ImagePath,
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
        public async Task<IActionResult> CreateDish([FromForm] DishDTO dishDTO)
        {
            try
            {

                // Save image if exists
                string imagePath = null;
                if (dishDTO.ImageFile != null)
                {
                    imagePath = await FileUpload.SaveImage("Images", dishDTO.ImageFile);
                }

                // Map DTO to entity
                var newDish = new Dish
                {
                    Name = dishDTO.Name,
                    Price = dishDTO.Price,
                    Status = dishDTO.Status,
                    ImagePath = imagePath
                };

                // Add to DbContext
                await _dbContext.Dishes.AddAsync(newDish);
                await _dbContext.SaveChangesAsync();

                return Created("success", new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Add Disk Successfully",
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

        // PUT api/dish/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDish(int id, [FromForm] DishDTO dishDTO)
        {
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

                // Update scalar properties
                existingDish.Name = dishDTO.Name;
                existingDish.Price = dishDTO.Price;
                existingDish.Status = dishDTO.Status;

                // Handle image update
                if (dishDTO.ImageFile != null)
                {
                    // Delete old image if it exists
                    if (!string.IsNullOrEmpty(existingDish.ImagePath))
                    {
                        FileUpload.DeleteImage(existingDish.ImagePath);
                    }

                    // Save new image and update ImagePath
                    existingDish.ImagePath = await FileUpload.SaveImage("Images", dishDTO.ImageFile);
                }
                // If diskDTO.ImageFile is null, do nothing, which will keep the existing image


                // Update entity in DbContext
                _dbContext.Dishes.Update(existingDish);
                await _dbContext.SaveChangesAsync();

                var updatedDishDTO = new DishDTO
                {
                    Id = existingDish.Id,
                    Name = existingDish.Name,
                    Price = existingDish.Price,
                    Status = existingDish.Status,
                    ImagePath = existingDish.ImagePath,
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
                    FileUpload.DeleteImage(dishToDelete.ImagePath);
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
