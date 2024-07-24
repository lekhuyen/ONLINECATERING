using APIRESPONSE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.DTOs;
using RESTAURANT.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomComboController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public CustomComboController(DatabaseContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomComboDTO>>> GetAllCustomCombos()
        {
            try
            {
                var customCombos = await _dbContext.CustomCombos.ToListAsync();

                var customComboDTOs = customCombos.Select(cc => new CustomComboDTO
                {
                    Id = cc.Id,
                    UserId = cc.UserId,
                    DishId = cc.DishId,
                    Date = cc.Date
                }).ToList();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get custom combos successfully",
                    Data = customComboDTOs
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

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetCustomComboById(int id)
        {
            try
            {
                var customCombo = await _dbContext.CustomCombos.FindAsync(id);

                if (customCombo == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Custom combo not found",
                    });
                }

                var customComboDTO = new CustomComboDTO
                {
                    Id = customCombo.Id,
                    UserId = customCombo.UserId,
                    DishId = customCombo.DishId,
                    Date = customCombo.Date
                };

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get custom combo successfully",
                    Data = customComboDTO
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Retrieve custom combo failed",
                    Data = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<CustomComboDTO>> CreateCustomCombo(CustomComboDTO customComboDTO)
        {
            try
            {
                // Check if UserId exists in Users table
                var existingUser = await _dbContext.Users.FindAsync(customComboDTO.UserId);
                if (existingUser == null)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Invalid UserId. User does not exist.",
                        Data = null
                    });
                }

                // Check if DishId exists in Dishes table
                var existingDish = await _dbContext.Dishes.FindAsync(customComboDTO.DishId);
                if (existingDish == null)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Invalid DishId. Dish does not exist.",
                        Data = null
                    });
                }

                // Map DTO to entity
                var newCustomCombo = new CustomCombo
                {
                    UserId = customComboDTO.UserId,
                    DishId = customComboDTO.DishId,
                    Date = customComboDTO.Date
                };

                // Add to DbContext
                await _dbContext.CustomCombos.AddAsync(newCustomCombo);
                await _dbContext.SaveChangesAsync();

                var updatedCustomComboDTO = new CustomComboDTO
                {
                    Id = newCustomCombo.Id,
                    UserId = newCustomCombo.UserId,
                    DishId = newCustomCombo.DishId,
                    Date = newCustomCombo.Date
                };

                return Created("success", new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Custom combo added successfully",
                    Data = updatedCustomComboDTO
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Error creating custom combo",
                    Data = e.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomCombo(int id, CustomComboDTO customComboDTO)
        {
            try
            {
                var existingCustomCombo = await _dbContext.CustomCombos.FindAsync(id);

                if (existingCustomCombo == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Custom combo not found",
                    });
                }

                // Check if UserId exists in Users table
                var existingUser = await _dbContext.Users.FindAsync(customComboDTO.UserId);
                if (existingUser == null)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Invalid UserId. User does not exist.",
                        Data = null
                    });
                }

                // Check if DishId exists in Dishes table
                var existingDish = await _dbContext.Dishes.FindAsync(customComboDTO.DishId);
                if (existingDish == null)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Invalid DishId. Dish does not exist.",
                        Data = null
                    });
                }

                // Update scalar properties
                existingCustomCombo.UserId = customComboDTO.UserId;
                existingCustomCombo.DishId = customComboDTO.DishId;
                existingCustomCombo.Date = customComboDTO.Date;

                // Update entity in DbContext
                _dbContext.CustomCombos.Update(existingCustomCombo);
                await _dbContext.SaveChangesAsync();

                var updatedCustomComboDTO = new CustomComboDTO
                {
                    Id = existingCustomCombo.Id,
                    UserId = existingCustomCombo.UserId,
                    DishId = existingCustomCombo.DishId,
                    Date = existingCustomCombo.Date
                };

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Custom combo updated successfully",
                    Data = updatedCustomComboDTO
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Error updating custom combo",
                    Data = e.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomCombo(int id)
        {
            try
            {
                var customComboToDelete = await _dbContext.CustomCombos.FindAsync(id);

                if (customComboToDelete == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Custom combo not found",
                    });
                }

                // Remove from DbContext and save changes
                _dbContext.CustomCombos.Remove(customComboToDelete);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Custom combo deleted successfully",
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Error deleting custom combo",
                    Data = e.Message
                });
            }
        }

        private bool CustomComboExists(int id)
        {
            return _dbContext.CustomCombos.Any(e => e.Id == id);
        }
    }
}
