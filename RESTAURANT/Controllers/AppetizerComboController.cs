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
    public class AppetizerComboController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public AppetizerComboController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAppetizerCombo()
        {
            try
            {
                var appetizers = await _dbContext.ComboAppetizers
                    .Include(c => c.Combo)
                    .Include(c => c.Appetizer)
                    .ToListAsync();

                var appetizersDTO = appetizers.Select(s => new ComboAppetizerDTO
                {
                    AppetizerId = s.Appetizer.Id,
                    AppetizerName = s.Appetizer.AppetizerName,
                    AppetizerPrice = s.Appetizer.Price,
                    AppetizerImage = s.Appetizer.AppetizerImage,

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
                    Message = "Get appetizers successfully",
                    Data = appetizersDTO
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
        public async Task<IActionResult> AddComboAppetizer(AddComboAppetizerDTO comboAppetizerDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var comboAppetizer = new ComboAppetizer
                    {
                        ComboId = comboAppetizerDTO.ComboId,
                        AppetizerId = comboAppetizerDTO.AppetizerId
                    };
                    await _dbContext.ComboAppetizers.AddAsync(comboAppetizer);
                    await _dbContext.SaveChangesAsync();

                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Status = 0,
                        Message = "Create comboAppetizer Successfully",
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
        public async Task<IActionResult> GetComboAppetizer(int comboid)
        {
            try
            {
                var appetizers = await _dbContext.ComboAppetizers
                    .Include(c => c.Combo)
                    .Include(c => c.Appetizer)
                    .Where(x => x.ComboId == comboid).ToListAsync();

                var appetizersDTO = appetizers.Select(s => new AppetizerDTO
                {
                    AppetizerId = s.Appetizer.Id,
                    AppetizerName = s.Appetizer.AppetizerName,
                    AppetizerPrice = s.Appetizer.Price,
                    AppetizerImage = s.Appetizer.AppetizerImage,
                }).ToList();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get appetizers successfully",
                    Data = appetizersDTO
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

        [HttpDelete]
        public async Task<IActionResult> DeleteComboAppetizer([FromQuery] int comboId, [FromQuery] int appetizerId)
        {
            try
            {
                // Find the ComboAppetizer with the specified ComboId and AppetizerId
                var comboAppetizer = await _dbContext.ComboAppetizers
                    .FirstOrDefaultAsync(ca => ca.ComboId == comboId && ca.AppetizerId == appetizerId);

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

                // Remove the ComboAppetizer from the database
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
            catch (Exception ex)
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
