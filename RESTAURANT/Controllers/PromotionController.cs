using APIRESPONSE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.DTOs;
using RESTAURANT.API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public PromotionController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Promotion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PromotionDTO>>> GetAllPromotions()
        {
            try
            {
                var promotions = await _dbContext.Promotions.ToListAsync();

                var promotionDTOs = promotions.Select(promotion => new PromotionDTO
                {
                    Id = promotion.Id,
                    OrderId = promotion.OrderId,
                    ComboId = promotion.ComboId,
                    Name = promotion.Name,
                    Description = promotion.Description,
                    ImagePath = promotion.ImagePath,
                    Status = promotion.Status,
                }).ToList();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get promotions successfully",
                    Data = promotionDTOs
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Internal server error",
                    Data = ex.Message
                });
            }
        }

        // GET: api/Promotion/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetPromotionById(int id)
        {
            try
            {
                var promotion = await _dbContext.Promotions.FindAsync(id);

                if (promotion == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Promotion not found",
                    });
                }

                var promotionDTO = new PromotionDTO
                {
                    Id = promotion.Id,
                    OrderId = promotion.OrderId,
                    Name = promotion.Name,
                    Description = promotion.Description,
                    ImagePath = promotion.ImagePath,
                    Status = promotion.Status,

                };

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get promotion successfully",
                    Data = promotionDTO
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

        // POST: api/Promotion
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreatePromotion([FromForm] PromotionDTO promotionDTO)
        {
            try
            {

                // Check if the OrderId exists in the Orders table
                var existingOrder = await _dbContext.Orders.FindAsync(promotionDTO.OrderId);
/*                if (existingOrder == null)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Invalid OrderId. Order does not exist.",
                        Data = null
                    });
                }*/

                // Save image if exists
                string imagePath = null;
                if (promotionDTO.ImageFile != null)
                {
                    imagePath = await FileUpload.SaveImage("Images", promotionDTO.ImageFile);
                }

                // Map DTO to entity
                var newPromotion = new Promotion
                {
                    Name = promotionDTO.Name,
                    Description = promotionDTO.Description,
                    ImagePath = imagePath,
                    ComboId = promotionDTO.ComboId,
                    Status = promotionDTO.Status,
                    OrderId = promotionDTO.OrderId  // Assign the OrderId
                };

                // Add to DbContext
                await _dbContext.Promotions.AddAsync(newPromotion);
                await _dbContext.SaveChangesAsync();

                // Prepare response DTO
                var createdPromotionDTO = new PromotionDTO
                {
                    Id = newPromotion.Id,
                    OrderId = newPromotion.OrderId,
                    Name = newPromotion.Name,
                    Description = newPromotion.Description,
                    ImagePath = newPromotion.ImagePath,
                    Status = newPromotion.Status,
              
                };

                return Created("success", new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Add promotion successfully",
                    Data = createdPromotionDTO
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

        // PUT: api/Promotion/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdatePromotion(int id, [FromForm] PromotionDTO promotionDTO)
        {
            try
            {
                var existingPromotion = await _dbContext.Promotions.FindAsync(id);

                if (existingPromotion == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Promotion not found",
                    });
                }

                if (id != promotionDTO.Id)
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
                existingPromotion.Name = promotionDTO.Name;
                existingPromotion.Description = promotionDTO.Description;
                existingPromotion.Status = promotionDTO.Status;
                existingPromotion.ComboId = promotionDTO.ComboId;
                

                // Handle image update
                if (promotionDTO.ImageFile != null)
                {
                    // Delete old image if it exists
                    if (!string.IsNullOrEmpty(existingPromotion.ImagePath))
                    {
                        FileUpload.DeleteImage(existingPromotion.ImagePath);
                    }

                    // Save new image and update ImagePath
                    existingPromotion.ImagePath = await FileUpload.SaveImage("Images", promotionDTO.ImageFile);
                }
                // If promotionDTO.ImageFile is null, do nothing, which will keep the existing image

                // Handle OrderId update if necessary (example scenario)
                if (promotionDTO.OrderId != existingPromotion.OrderId)
                {
                    // Check if the new OrderId exists
                    var existingOrder = await _dbContext.Orders.FindAsync(promotionDTO.OrderId);
                    if (existingOrder == null)
                    {
                        return BadRequest(new ApiResponse
                        {
                            Success = false,
                            Status = 1,
                            Message = "Invalid OrderId. Order does not exist.",
                            Data = null
                        });
                    }

                    existingPromotion.OrderId = promotionDTO.OrderId; // Update the OrderId
                }

                // Update entity in DbContext
                _dbContext.Promotions.Update(existingPromotion);
                await _dbContext.SaveChangesAsync();

                // Prepare response DTO
                var updatedPromotionDTO = new PromotionDTO
                {
                    Id = existingPromotion.Id,
/*                    OrderId = existingPromotion.OrderId,*/
                    Name = existingPromotion.Name,
                    Description = existingPromotion.Description,
                    ImagePath = existingPromotion.ImagePath,
                    Status = existingPromotion.Status,

                };

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Promotion updated successfully",
                    Data = updatedPromotionDTO
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Error from service: " + ex.Message,
                    Data = null
                });
            }
        }


        // DELETE: api/Promotion/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeletePromotion(int id)
        {
            try
            {
                var promotionToDelete = await _dbContext.Promotions.FindAsync(id);

                if (promotionToDelete == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Promotion not found",
                    });
                }

                // Delete associated image if exists
                if (!string.IsNullOrEmpty(promotionToDelete.ImagePath))
                {
                    FileUpload.DeleteImage(promotionToDelete.ImagePath);
                }

                // Remove from DbContext and save changes
                _dbContext.Promotions.Remove(promotionToDelete);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Delete promotion successfully",
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

        // Helper method to check if a promotion exists
        private bool PromotionExists(int id)
        {
            return _dbContext.Promotions.Any(e => e.Id == id);
        }
    }
}
