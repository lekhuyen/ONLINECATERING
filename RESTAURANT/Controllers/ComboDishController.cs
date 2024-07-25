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
                    DishStatus = cd.Dish.Status,
                    DishImagePath = cd.Dish.ImagePath,

                    ComboId = cd.Combo.Id,
                    ComboName = cd.Combo.Name,
                    ComboPrice = cd.Combo.Price,
                    ComboStatus = cd.Combo.Status,
                    ComboType = cd.Combo.Type,
                    ComboImagePath = cd.Combo.ImagePath
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
        public async Task<IActionResult> Create(ComboDishDTO comboDishDTO)
        {
            try
            {
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

                var newComboDish = new ComboDish
                {
                    DishId = comboDishDTO.DishId,
                    ComboId = comboDishDTO.ComboId
                };

                _dbContext.ComboDishes.Add(newComboDish);
                await _dbContext.SaveChangesAsync();

                var createdComboDishDTO = new ComboDishDTO
                {
                    DishId = newComboDish.DishId,
                    DishName = dish?.Name,
                    DishPrice = dish?.Price,
                    DishStatus = dish?.Status,
                    DishImagePath = dish?.ImagePath,

                    ComboId = newComboDish.ComboId,
                    ComboName = combo?.Name,
                    ComboPrice = combo?.Price,
                    ComboStatus = combo?.Status,
                    ComboType = combo?.Type,
                    ComboImagePath = combo?.ImagePath
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
