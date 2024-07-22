using APIRESPONSE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.DTOs;
using RESTAURANT.API.Models;
using RESTAURANT.API.Repositories;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComboController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public ComboController(DatabaseContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComboDTO>>> GetAllCombos()
        {
            try
            {
                var combos = await _dbContext.Combos.ToListAsync();

                var ComboDTOs = combos.Select(combo => new ComboDTO
                {
                    Id = combo.Id,
                    Name = combo.Name,
                    Price = combo.Price,
                    Status = combo.Status,
                    ImagePath = combo.ImagePath,
                    Type = combo.Type,

                }).ToList();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get combos successfully",
                    Data = ComboDTOs
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Internal server error",
                    Data = e.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetComboById(int id)
        {
            try
            {
                var combo = await _dbContext.Combos
                    .FindAsync(id);

                if (combo == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Combo not found",
                    });
                }

                var comboDTO = new ComboDTO
                {
                    Id = combo.Id,
                    Name = combo.Name,
                    Price = combo.Price,
                    Status = combo.Status,
                    ImagePath = combo.ImagePath,
                    Type = combo.Type,

                };

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get combo successfully",
                    Data = comboDTO
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Create combo failed"
                });
            }
        }



        [HttpPost]
        public async Task<ActionResult<ComboDTO>> CreateCombo([FromForm] ComboDTO comboDTO)
        {
            try
            {

                // Save image if exists
                string imagePath = null;
                if (comboDTO.ImageFile != null)
                {
                    imagePath = await FileUpload.SaveImage("Images", comboDTO.ImageFile);
                }

                // Map DTO to entity
                var newCombo = new Combo
                {
                    Name = comboDTO.Name,
                    Price = comboDTO.Price,
                    Status = comboDTO.Status,
                    ImagePath = imagePath,
                    Type = comboDTO.Type,
                };

                // Add to DbContext
                await _dbContext.Combos.AddAsync(newCombo);
                await _dbContext.SaveChangesAsync();

                var updatedComboDTO = new ComboDTO
                {
                    Id = newCombo.Id,
                    Name = newCombo.Name,
                    Price = newCombo.Price,
                    Status = newCombo.Status,
                    ImagePath = newCombo.ImagePath, // Ensure this matches the updated value
                    Type = newCombo.Type,
                };

                return Created("success", new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Add Combo Successfully",
                    Data = updatedComboDTO
                });
            }
            catch (Exception e)
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCombo(int id, [FromForm] ComboDTO comboDTO)
        {
            try
            {
                var existingCombo = await _dbContext.Combos.FindAsync(id);

                if (existingCombo == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Combo not found",
                    });
                }

                if (id != comboDTO.Id)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Mismatched ID in object and parameter",
                        Data = null
                    });
                }

                // Update scalar properties
                existingCombo.Name = comboDTO.Name;
                existingCombo.Price = comboDTO.Price;
                existingCombo.Status = comboDTO.Status;
                existingCombo.Type = comboDTO.Type;

                // Handle image update
                if (comboDTO.ImageFile != null)
                {
                    // Delete old image if it exists
                    if (!string.IsNullOrEmpty(existingCombo.ImagePath))
                    {
                        FileUpload.DeleteImage(existingCombo.ImagePath);
                    }

                    // Save new image and update ImagePath
                    existingCombo.ImagePath = await FileUpload.SaveImage("Images", comboDTO.ImageFile);
                }
                // If comboDTO.ImageFile is null, do nothing, which will keep the existing image

                // Update entity in DbContext
                _dbContext.Combos.Update(existingCombo);
                await _dbContext.SaveChangesAsync();

                var updatedComboDTO = new ComboDTO
                {
                    Id = existingCombo.Id,
                    Name = existingCombo.Name,
                    Price = existingCombo.Price,
                    Status = existingCombo.Status,
                    ImagePath = existingCombo.ImagePath, // Ensure this matches the updated value
                    Type = existingCombo.Type,
                };

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Combo updated successfully",
                    Data = updatedComboDTO
                });
            }
            catch (Exception e)
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


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCombo(int id)
        {
            try
            {
                var combotoDelete = await _dbContext.Combos.FindAsync(id);
                if (combotoDelete == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Combo not found",
                    });
                }

                // Delete associated image if exists
                if (!string.IsNullOrEmpty(combotoDelete.ImagePath))
                {
                    FileUpload.DeleteImage(combotoDelete.ImagePath);
                }

                // Remove from DbContext and save changes
                _dbContext.Combos.Remove(combotoDelete);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Delete combo successfully",
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


        private bool ComboExists(int id)
        {
            return _dbContext.Combos.Any(e => e.Id == id);
        }
    }
}
