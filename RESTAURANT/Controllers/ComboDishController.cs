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
        [HttpGet]
        public async Task<IActionResult> GetAllComboDish()
        {
            try
            {
                var comboDishes = await _dbContext.ComboDishes
                    .Include(cd => cd.Combo)
                    .Include(cd => cd.Dish)
                    .ToListAsync();

                var list = comboDishes.Select(cd => new ComboDishDTO
                {
                    DishId = cd.DishId,
                    ComboId = cd.ComboId,
/*                    DishName = cd.Dish.Name,
                    DishPrice = cd.Dish.Price, 
                    ComboName = cd.Combo.Name,
                    ComboPrice = cd.Combo.Price,  */
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

        // POST: api/ComboDish/Create
        [HttpPost("Create")]
        public async Task<IActionResult> Create(ComboDishDTO comboDishDTO)
        {
            try
            {
                // Validate if DishId and ComboId exist
                var dish = await _dbContext.Dishes.FindAsync(comboDishDTO.DishId);
                var combo = await _dbContext.Combos.FindAsync(comboDishDTO.ComboId);

                if (dish == null || combo == null)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Invalid DishId or ComboId provided",
                        Data = null
                    });
                }

                // Create new ComboDish entity and save
                var newComboDish = new ComboDish
                {
                    DishId = comboDishDTO.DishId,
                    ComboId = comboDishDTO.ComboId
                };

                _dbContext.ComboDishes.Add(newComboDish);
                await _dbContext.SaveChangesAsync();

                // Return newly created ComboDishDTO with populated DishName, DishPrice, ComboName, and ComboPrice
                var createdComboDishDTO = new ComboDishDTO
                {
                    DishId = newComboDish.DishId,
                    ComboId = newComboDish.ComboId,
/*                    DishName = dish.Name,
                    DishPrice = dish.Price, 
                    ComboName = combo.Name,
                    ComboPrice = combo.Price, */
                };

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "ComboDish created successfully",
                    Data = createdComboDishDTO
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

    }

}
