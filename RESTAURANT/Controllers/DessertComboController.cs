using APIRESPONSE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.DTOs;
using RESTAURANT.API.Models;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DessertComboController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public DessertComboController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDessertCombo()
        {
            try
            {
                var appetizers = await _dbContext.ComboDesserts
                    .Include(c => c.Combo)
                    .Include(c => c.Dessert)
                    .ToListAsync();

                var dessertDTO = appetizers.Select(s => new ComboDessertDTO
                {
                    DessertId = s.Dessert.Id,
                    DessertName = s.Dessert.DessertName,
                    DessertPrice = s.Dessert.Price,
                    DessertImage = s.Dessert.DessertImage,

                    ComboId = s.Combo.Id,
                    ComboPrice = s.Combo.Price,
                    ComboName = s.Combo.Name,
                    ComboImagePath = s.Combo.ImagePath,
                    ComboType = s.Combo.Type,
                }).ToList();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get dessert successfully",
                    Data = dessertDTO
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
        public async Task<IActionResult> AddComboAppetizer(AddDessertDTO dessertDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var comboDessert = new ComboDessert
                    {
                        ComboId = dessertDTO.ComboId,
                        DessertId = dessertDTO.DessertId
                    };
                    await _dbContext.ComboDesserts.AddAsync(comboDessert);
                    await _dbContext.SaveChangesAsync();

                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Status = 0,
                        Message = "Create comboDessert Successfully",
                    });
                }
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Empty",
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

        [HttpGet("{comboid}")]
        public async Task<IActionResult> GetComboDessert(int comboid)
        {
            try
            {
                var dessert = await _dbContext.ComboDesserts
                    .Include(c => c.Combo)
                    .Include(c => c.Dessert)
                    .Where(x => x.ComboId == comboid).ToListAsync();

                var dessertDTO = dessert.Select(s => new DessertDTO
                {
                    Id = s.Dessert.Id,
                    DessertName = s.Dessert.DessertName,
                    Price = s.Dessert.Price,
                    DessertImage = s.Dessert.DessertImage,
                }).ToList();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get dessert successfully",
                    Data = dessertDTO
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
    }
}
