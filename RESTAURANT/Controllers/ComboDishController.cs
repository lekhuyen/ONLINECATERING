using APIRESPONSE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.DTOs;
using RESTAURANT.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;

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

        // GET: api/ComboDish/Index
        // GET: api/ComboDish/Index
        [HttpGet]
        public async Task<IActionResult> GetAllComboDish()
        {
            try
            {
                var comboDishes = await _dbContext.ComboDishes
                    .Include(cd => cd.Dish)
                        .ThenInclude(d => d.CustomCombos)
                    .Include(cd => cd.Combo)
                    .ToListAsync();

                var list = comboDishes.Select(cd => new ComboDishDTO
                {
                    DishId = cd.Dish.Id,
                    DishName = cd.Dish.Name,
                    DishPrice = cd.Dish.Price,

                    DishImagePath = cd.Dish.ImagePath,

                    ComboId = cd.Combo.Id,
                    ComboName = cd.Combo.Name,
                    ComboPrice = cd.Combo.Price,
                    ComboStatus = cd.Combo.Status,
                    ComboType = cd.Combo.Type,

                }).ToList();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get ComboDishes successfully",
                    Data = list
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Error from service",
                    Data = ex.Message
                });
            }
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create(AddComboDishDTO comboDishDTO)
        {
            try
            {
                 var createdComboDish= new ComboDish
                {

                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "ComboDish not found",
                        Data = null
                    });
                }

                var comboDishDTO = new ComboDishDTO
                {
                    ComboDishId = comboDish.Id,
                    DishId = comboDish.Dish.Id,
                    DishName = comboDish.Dish.Name,
                    DishPrice = comboDish.Dish.Price,
                    DishImagePath = comboDish.Dish.ImagePath,

                    ComboId = comboDish.Combo.Id,
                    ComboName = comboDish.Combo.Name,
                    ComboPrice = comboDish.Combo.Price,
                    ComboImagePath = comboDish.Combo.ImagePath,
                    ComboType = comboDish.Combo.Type,


                    DishId = comboDishDTO.DishId,
                    
                    ComboId = comboDishDTO.ComboId,
                    

                };
                _dbContext.ComboDishes.Add(createdComboDish);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "ComboDish created successfully",
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Error occurred while creating ComboDish",
                    Data = ex.Message
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

                    Id = dto.ComboDishId,
                    ComboId = dto.ComboId,
                    DishId = dto.DishId
                };

                await _dbContext.ComboDishes.AddAsync(comboDish);
                await _dbContext.SaveChangesAsync();

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

        [HttpDelete("Delete/{comboId}/{dishId}")]
        public async Task<IActionResult> DeleteComboDish(int comboId, int dishId)
        {
            try
            {
                // Find the ComboDish entry with the specified ComboId and DishId
                var comboDish = await _dbContext.ComboDishes
                    .FirstOrDefaultAsync(cd => cd.ComboId == comboId && cd.DishId == dishId);

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

                // Remove the ComboDish entry from the database
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


    }

}
