using APIRESPONSE.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.DTOs;
using RESTAURANT.API.Helpers;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ComboController(DatabaseContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComboDTO>>> GetAllCombos()
        {
            try
            {
                var combos = await _dbContext.Combos
                    .Include(c => c.Promotions)
                    .ToListAsync();

                var ComboDTOs = combos.Select(combo => new ComboDTO
                {
                    Id = combo.Id,
                    Name = combo.Name,
                    Price = combo.Price,
                    Status = combo.Status,
                    ImagePath = combo.ImagePath,
                    Type = combo.Type,
                    Promotions = combo.Promotions.Select(p => new PromotionDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        ImagePath= p.ImagePath,
                    }).ToList(),
                    
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
                    .Include(c => c.Promotions)
                    .FirstOrDefaultAsync(x => x.Id == id);

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
                    Promotions = combo.Promotions.Select(p => new PromotionDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        ImagePath = p.ImagePath,
                    }).ToList(),

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
        public async Task<ActionResult<ComboDTO>> CreateCombo([FromForm] ComboDTO comboDTO, IFormFile formFile)
        {
            var fileUpload = new FileUpload(_webHostEnvironment);

            try
            {

                // Save image if exists
                string result = null;
                if (formFile != null)
                {
                     result = await fileUpload.SaveImage("images", formFile);
                }

                // Map DTO to entity
                var newCombo = new Combo
                {
                    Name = comboDTO.Name,
                    Price = comboDTO.Price,
                    Status = comboDTO.Status,
                    ImagePath = result,
                    Type = (int)comboDTO.Type,
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
        public async Task<IActionResult> UpdateCombo(int id, [FromForm] ComboDTO comboDTO, IFormFile? formFile)
        {
            var fileUpload = new FileUpload(_webHostEnvironment);

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
                existingCombo.Type = (int)comboDTO.Type;

                // Handle image update
                if (formFile != null)
                {
                    // Delete old image if it exists
                    if (!string.IsNullOrEmpty(existingCombo.ImagePath))
                    {
                        // Optionally, you can implement image deletion logic here if necessary
                        fileUpload.DeleteImage(existingCombo.ImagePath);
                    }

                    // Save new image and update ImagePath
                    existingCombo.ImagePath = await fileUpload.SaveImage("images", formFile);
                }
                // If imageFile is null, keep the existing image

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
            var fileUpload = new FileUpload(_webHostEnvironment);

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

                if (!string.IsNullOrEmpty(combotoDelete.ImagePath))
                {
                    fileUpload.DeleteImage(combotoDelete.ImagePath);
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
