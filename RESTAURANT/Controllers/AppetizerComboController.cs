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
                    Id = s.Appetizer.Id,
                    Name = s.Appetizer.AppetizerName,
                    Price = s.Appetizer.Price,
                    Image = s.Appetizer.AppetizerImage,
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

                // Check if AppetizerId exists
                var appetizerExists = await _dbContext.Appetizers
                    .AnyAsync(a => a.Id == dto.AppetizerId);

                if (!appetizerExists)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "AppetizerId does not exist",
                        Data = null
                    });
                }

                // Check if a ComboAppetizer with the same ComboId and AppetizerId already exists
                var existingComboAppetizer = await _dbContext.ComboAppetizers
                    .AnyAsync(ca => ca.ComboId == dto.ComboId && ca.AppetizerId == dto.AppetizerId);

                if (existingComboAppetizer)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "A ComboAppetizer with the same ComboId and AppetizerId already exists",
                        Data = null
                    });
                }

                var comboAppetizer = new ComboAppetizer
                {
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

        //bỏ 
        [HttpPost("order-create-comboApettizer")]
        public async Task<IActionResult> CreateComboAppetizerOrder(List<AddComboAppetizerDTO> comboAppetizerDTO)
        {
            //[FromBody]
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

                foreach (var dto in comboAppetizerDTO)
                {
                    var existsAppetzer = await _dbContext.ComboAppetizers
                    .FirstOrDefaultAsync(ca => ca.ComboId == dto.ComboId);

                    if (existsAppetzer == null)
                    {
                        var comboAppetizer = new ComboAppetizer
                        {
                            ComboId = dto.ComboId,
                            AppetizerId = dto.AppetizerId,
                            Quantity = dto.Quantity,
                        };

                        await _dbContext.ComboAppetizers.AddAsync(comboAppetizer);
                        
                    }
                    else
                    {
                        existsAppetzer.Quantity = dto.Quantity;
                        existsAppetzer.AppetizerId = dto.AppetizerId;
                        _dbContext.ComboAppetizers.Update(existsAppetzer);
                    }

                }
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