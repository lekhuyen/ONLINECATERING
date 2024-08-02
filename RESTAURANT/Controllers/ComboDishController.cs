using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.DTOs;
using RESTAURANT.API.Models;
using APIRESPONSE.Models;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComboDishController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public ComboDishController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComboDishes()
        {
            try
            {
                var comboDishes = await _dbContext.ComboDishes
                    .Include(cd => cd.Combo)
                    .Include(cd => cd.Dish)
                    .ToListAsync();

                var comboDishesDTO = comboDishes.Select(cd => new ComboDishDTO
                {
                    ComboDishId = cd.Id,
                    DishId = cd.Dish.Id,
                    DishName = cd.Dish.Name,
                    DishPrice = cd.Dish.Price,

                    DishImagePath = cd.Dish.ImagePath,

                    ComboId = cd.Combo.Id,
                    ComboName = cd.Combo.Name,
                    ComboPrice = cd.Combo.Price,
                    ComboImagePath = cd.Combo.ImagePath,
                    ComboType = cd.Combo.Type,
                }).ToList();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Successfully retrieved all combo dishes",
                    Data = comboDishesDTO
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = $"Internal server error: {e.Message}",
                    Data = null
                });
            }
        }

        [HttpGet("{comboid}")]
        public async Task<IActionResult> GetComboDessert(int comboid)
        {
            try
            {
                var dish = await _dbContext.ComboDishes
                    .Include(c => c.Combo)
                    .Include(c => c.Dish)
                    .Where(x => x.ComboId == comboid).ToListAsync();

                var dishDTO = dish.Select(s => new DishDTO
                {
                    Id = s.Dish.Id,
                    Name = s.Dish.Name,
                    Price = s.Dish.Price,
                    ImagePath = s.Dish.ImagePath,
                }).ToList();
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
                    Data = null
                });
            }
        }



        [HttpPost]
        public async Task<IActionResult> CreateComboDish([FromBody] AddComboDishDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Invalid data",
                        Data = null
                    });
                }

                // Check if ComboId exists
                var comboExists = await _dbContext.Combos
                    .AnyAsync(c => c.Id == dto.ComboId);

                if (!comboExists)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "ComboId does not exist",
                        Data = null
                    });
                }

                // Check if DishId exists
                var dishExists = await _dbContext.Dishes
                    .AnyAsync(d => d.Id == dto.DishId);

                if (!dishExists)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "DishId does not exist",
                        Data = null
                    });
                }

                // Check if a ComboDish with the same ComboId and DishId already exists
                var existingComboDish = await _dbContext.ComboDishes
                    .AnyAsync(cd => cd.ComboId == dto.ComboId && cd.DishId == dto.DishId);

                if (existingComboDish)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "A ComboDish with the same ComboId and DishId already exists",
                        Data = null
                    });
                }

                var comboDish = new ComboDish
                {
                    ComboId = dto.ComboId,
                    DishId = dto.DishId,

                };

                await _dbContext.ComboDishes.AddAsync(comboDish);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "ComboDish created successfully",
                    Data = null
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = $"Error occurred: {e.Message}",
                    Data = null
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComboDish(int id)
        {
            try
            {
                var comboDish = await _dbContext.ComboDishes
                    .FirstOrDefaultAsync(cd => cd.Id == id);

                if (comboDish == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "ComboDish not found",
                        Data = null
                    });
                }

                _dbContext.ComboDishes.Remove(comboDish);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "ComboDish deleted successfully",
                    Data = null
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = $"Internal server error: {e.Message}",
                    Data = null
                });
            }
        }
    }
}
