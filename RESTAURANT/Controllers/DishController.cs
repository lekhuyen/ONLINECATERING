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
                    ComboDishes = dish.ComboDishes.Select(cd => new ComboDishDTO
                    {
                        ComboId = cd.ComboId,
                        DishId = cd.DishId
                    }).ToList(),
                    CustomCombo = new CustomComboDTO // Sample, adjust as needed
                    {
                        Id = dish.CustomCombo?.Id ?? 0,
                        Date = dish.CustomCombo?.Date ?? DateTime.MinValue,
                        // Map other properties from CustomComboDTO if needed
                    }
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
                    .Include(d => d.ComboDishes)
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
                    ImagePath = dish.ImagePath,
                    ComboDishes = dish.ComboDishes.Select(cd => new ComboDishDTO
                    {
                        ComboId = cd.ComboId,
                        DishId = cd.DishId
                    }).ToList(),
                    CustomCombo = new CustomComboDTO // Sample, adjust as needed
                    {
                        Id = dish.CustomCombo?.Id ?? 0,
                        Date = dish.CustomCombo?.Date ?? DateTime.MinValue,
                        // Map other properties from CustomComboDTO if needed
                    }
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
                // Handle CustomCombo and ComboDishes if needed
                // For simplicity, assume no CustomCombo and ComboDishes in this example

                // Save image if exists
                string imagePath = null;
                if (dishDTO.ImageFile != null)
                {
                    imagePath = await FileUpload.SaveImage("dishImages", dishDTO.ImageFile);
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

                var createdDishDTO = new DishDTO
                {
                    Id = newDish.Id,
                    Name = newDish.Name,
                    Price = newDish.Price,
                    Status = newDish.Status,
                    ImagePath = newDish.ImagePath,
                    ComboDishes = new List<ComboDishDTO>(), // Empty for now, adjust if needed
                    CustomCombo = null // No CustomCombo in create scenario
                };

                return CreatedAtAction(nameof(GetDishById), new { id = newDish.Id }, new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Dish created successfully",
                    Data = createdDishDTO
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
                    existingDish.ImagePath = await FileUpload.SaveImage("dishImages", dishDTO.ImageFile);
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
                    ImagePath = existingDish.ImagePath,
                    ComboDishes = existingDish.ComboDishes.Select(cd => new ComboDishDTO
                    {
                        ComboId = cd.ComboId,
                        DishId = cd.DishId
                    }).ToList(),
                    CustomCombo = new CustomComboDTO // Sample, adjust as needed
                    {
                        Id = existingDish.CustomCombo?.Id ?? 0,
                        Date = existingDish.CustomCombo?.Date ?? DateTime.MinValue,
                        // Map other properties from CustomComboDTO if needed
                    }
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
