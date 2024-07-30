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
    public class AppetizerComboController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public AppetizerComboController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppetizerCombos()
        {
            try
            {
                var combos = await _dbContext.ComboAppetizers
                    .Include(c => c.Combo)
                    .Include(c => c.Appetizer)
                    .ToListAsync();

                var combosDTO = combos.Select(ca => new ComboAppetizerDTO
                {
                    ComboAppetizerId = ca.Id,
                    AppetizerId = ca.Appetizer.Id,
                    AppetizerName = ca.Appetizer.AppetizerName,
                    AppetizerPrice = ca.Appetizer.Price,
                    AppetizerQuantity = ca.Appetizer.Quantity,
                    AppetizerImage = ca.Appetizer.AppetizerImage,

                    ComboId = ca.Combo.Id,
                    ComboName = ca.Combo.Name,
                    ComboPrice = ca.Combo.Price,
                    ComboImagePath = ca.Combo.ImagePath,
                    ComboType = ca.Combo.Type,
                    Status = ca.Combo.Status
                }).ToList();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Successfully retrieved all combos",
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetComboAppetizerById(int id)
        {
            try
            {
                var comboAppetizer = await _dbContext.ComboAppetizers
                    .Include(c => c.Combo)
                    .Include(c => c.Appetizer)
                    .FirstOrDefaultAsync(ca => ca.Id == id);

                if (comboAppetizer == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "ComboAppetizer not found",
                        Data = null
                    });
                }

                var comboAppetizerDTO = new ComboAppetizerDTO
                {
                    
                    AppetizerId = comboAppetizer.Appetizer.Id,
                    AppetizerName = comboAppetizer.Appetizer.AppetizerName,
                    AppetizerPrice = comboAppetizer.Appetizer.Price,
                    AppetizerQuantity = comboAppetizer.Appetizer.Quantity,
                    AppetizerImage = comboAppetizer.Appetizer.AppetizerImage,

                    ComboId = comboAppetizer.Combo.Id,
                    ComboName = comboAppetizer.Combo.Name,
                    ComboPrice = comboAppetizer.Combo.Price,
                    ComboImagePath = comboAppetizer.Combo.ImagePath,
                    ComboType = comboAppetizer.Combo.Type,
                    Status = comboAppetizer.Combo.Status
                };

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Successfully retrieved the combo appetizer",
                    Data = comboAppetizerDTO
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

        [HttpPost]
        public async Task<IActionResult> CreateComboAppetizer([FromBody] AddComboAppetizerDTO dto)
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

                var comboAppetizer = new ComboAppetizer
                {
                    Id = dto.ComboAppetizerId,
                    ComboId = dto.ComboId,
                    AppetizerId = dto.AppetizerId
                };

                await _dbContext.ComboAppetizers.AddAsync(comboAppetizer);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "ComboAppetizer created successfully",
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
        public async Task<IActionResult> DeleteComboAppetizer(int id)
        {
            try
            {
                var comboAppetizer = await _dbContext.ComboAppetizers
                    .FirstOrDefaultAsync(ca => ca.Id == id);

                if (comboAppetizer == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "ComboAppetizer not found",
                        Data = null
                    });
                }

                _dbContext.ComboAppetizers.Remove(comboAppetizer);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "ComboAppetizer deleted successfully",
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
