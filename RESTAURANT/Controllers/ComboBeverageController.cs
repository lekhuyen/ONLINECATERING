using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
    public class ComboBeverageController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public ComboBeverageController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComboBeverages()
        {
            try
            {
                var comboBeverages = await _dbContext.ComboBeverages
                    .Include(cb => cb.Combo)
                    .Include(cb => cb.Beverage)
                    .ToListAsync();

                var comboBeveragesDTO = comboBeverages.Select(cb => new ComboBeverageDTO
                {
                    ComboBeverageId = cb.Id,
                    BeverageId = cb.Beverage.Id,
                    BeverageName = cb.Beverage.BeverageName,
                    BeveragePrice = cb.Beverage.Price,
                    BeverageQuantity = cb.Beverage.Quantity,
                    BeverageImage = cb.Beverage.BeverageImage,

                    ComboId = cb.Combo.Id,
                    ComboName = cb.Combo.Name,
                    ComboPrice = cb.Combo.Price,
                    ComboImagePath = cb.Combo.ImagePath,
                    ComboType = cb.Combo.Type,
                    Status = cb.Combo.Status
                }).ToList();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Successfully retrieved all combo beverages",
                    Data = comboBeveragesDTO
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
        public async Task<IActionResult> GetComboBeverages(int comboid)
        {
            try
            {
                var beverages = await _dbContext.ComboBeverages
                    .Include(cb => cb.Combo)
                    .Include(cb => cb.Beverage)
                    .Where(cb => cb.ComboId == comboid)
                    .ToListAsync();

                var beveragesDTO = beverages.Select(b => new BeverageDTO
                {
                    Id = b.Beverage.Id,
                    Name = b.Beverage.BeverageName,
                    Price = b.Beverage.Price,
                    Image = b.Beverage.BeverageImage,
                }).ToList();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get beverages successfully",
                    Data = beveragesDTO
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
        public async Task<IActionResult> CreateComboBeverage([FromBody] AddComboBeverageDTO dto)
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

                // Check if BeverageId exists
                var beverageExists = await _dbContext.Beverages
                    .AnyAsync(b => b.Id == dto.BeverageId);

                if (!beverageExists)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "BeverageId does not exist",
                        Data = null
                    });
                }

                // Check if a ComboBeverage with the same ComboId and BeverageId already exists
                var existingComboBeverage = await _dbContext.ComboBeverages
                    .AnyAsync(cb => cb.ComboId == dto.ComboId && cb.BeverageId == dto.BeverageId);

                if (existingComboBeverage)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "A ComboBeverage with the same ComboId and BeverageId already exists",
                        Data = null
                    });
                }

                var comboBeverage = new ComboBeverage
                {
                    Id = dto.ComboBeverageId,
                    ComboId = dto.ComboId,
                    BeverageId = dto.BeverageId
                };

                await _dbContext.ComboBeverages.AddAsync(comboBeverage);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "ComboBeverage created successfully",
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
        public async Task<IActionResult> DeleteComboBeverage(int id)
        {
            try
            {
                var comboBeverage = await _dbContext.ComboBeverages
                    .FirstOrDefaultAsync(cb => cb.Id == id);

                if (comboBeverage == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "ComboBeverage not found",
                        Data = null
                    });
                }

                _dbContext.ComboBeverages.Remove(comboBeverage);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "ComboBeverage deleted successfully",
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
