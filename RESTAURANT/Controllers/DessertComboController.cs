using System;
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
    public class DessertComboController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public DessertComboController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDessertCombos()
        {
            try
            {
                var combos = await _dbContext.ComboDesserts
                    .Include(cd => cd.Combo)
                    .Include(cd => cd.Dessert)
                    .ToListAsync();

                var combosDTO = combos.Select(cd => new ComboDessertDTO
                {
                    ComboDessertId = cd.Id,
                    DessertId = cd.Dessert.Id,
                    DessertName = cd.Dessert.DessertName,
                    DessertPrice = cd.Dessert.Price,
                    DessertQuantity = cd.Dessert.Quantity,
                    DessertImage = cd.Dessert.DessertImage,

                    ComboId = cd.Combo.Id,
                    ComboName = cd.Combo.Name,
                    ComboPrice = cd.Combo.Price,
                    ComboImagePath = cd.Combo.ImagePath,
                    ComboType = cd.Combo.Type
                }).ToList();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Successfully retrieved all dessert combos",
                    Data = combosDTO
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

        [HttpPost]
        public async Task<IActionResult> CreateComboDessert([FromBody] AddComboDessertDTO dto)
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

                // Check if DessertId exists
                var dessertExists = await _dbContext.Desserts
                    .AnyAsync(d => d.Id == dto.DessertId);

                if (!dessertExists)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "DessertId does not exist",
                        Data = null
                    });
                }

                // Check if a ComboDessert with the same ComboId and DessertId already exists
                var existingComboDessert = await _dbContext.ComboDesserts
                    .AnyAsync(cd => cd.ComboId == dto.ComboId && cd.DessertId == dto.DessertId);

                if (existingComboDessert)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "A ComboDessert with the same ComboId and DessertId already exists",
                        Data = null
                    });
                }

                var comboDessert = new ComboDessert
                {
                    Id = dto.ComboDessertId,
                    ComboId = dto.ComboId,
                    DessertId = dto.DessertId
                };

                await _dbContext.ComboDesserts.AddAsync(comboDessert);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "ComboDessert created successfully",
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
        public async Task<IActionResult> DeleteComboDessert(int id)
        {
            try
            {
                var comboDessert = await _dbContext.ComboDesserts
                    .FirstOrDefaultAsync(cd => cd.Id == id);

                if (comboDessert == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "ComboDessert not found",
                        Data = null
                    });
                }

                _dbContext.ComboDesserts.Remove(comboDessert);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "ComboDessert deleted successfully",
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