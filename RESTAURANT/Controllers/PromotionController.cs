using APIRESPONSE.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.DTOs;
using RESTAURANT.API.Helpers;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PromotionController(DatabaseContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: api/Promotion
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllPromotions()
        {
            try
            {
                var promotions = await _dbContext.Promotions.ToListAsync();

                var promotionDTOs = promotions.Select(promotion => new PromotionDTO
                {
                    Id = promotion.Id,
                    Name = promotion.Name,
                    Description = promotion.Description,
                    ImagePath = promotion.ImagePath,
                    Status = promotion.Status,
                    QuantityTable = promotion.QuantityTable,
                    Price = promotion.Price
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
                    Name = promotion.Name,
                    Description = promotion.Description,
                    ImagePath = promotion.ImagePath,
                    Status = promotion.Status,
                    QuantityTable = promotion.QuantityTable,
                    Price = promotion.Price
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
        public async Task<ActionResult<ApiResponse>> CreatePromotion([FromForm] PromotionDTO promotionDTO, IFormFile? formFile)
        {
            var fileUpload = new FileUpload(_webHostEnvironment);
            try
            {
                // Save image if exists
                string imagePath = null;
                if (formFile != null)
                {
                    imagePath = await fileUpload.SaveImage("images", formFile);
                }

                // Map DTO to entity
                var newPromotion = new Promotion
                {
                    Name = promotionDTO.Name,
                    Description = promotionDTO.Description,
                    ImagePath = imagePath,
                    Status = promotionDTO.Status,
                    QuantityTable = promotionDTO.QuantityTable,
                    Price = promotionDTO.Price
                };

                // Add to DbContext
                await _dbContext.Promotions.AddAsync(newPromotion);
                await _dbContext.SaveChangesAsync();

                // Prepare response DTO
                var createdPromotionDTO = new PromotionDTO
                {
                    Id = newPromotion.Id,
                    Name = newPromotion.Name,
                    Description = newPromotion.Description,
                    ImagePath = newPromotion.ImagePath,
                    Status = newPromotion.Status,
                    QuantityTable = newPromotion.QuantityTable,
                    Price = newPromotion.Price
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
        // PUT: api/Promotion/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdatePromotion(int id, [FromForm] PromotionDTO promotionDTO, IFormFile? formFile)
        {
            var fileUpload = new FileUpload(_webHostEnvironment);

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
                existingPromotion.QuantityTable = promotionDTO.QuantityTable;
                existingPromotion.Price = promotionDTO.Price;

                // Handle image update
                if (formFile != null)
                {
                    // Delete old image if it exists
                    if (!string.IsNullOrEmpty(existingPromotion.ImagePath))
                    {
                        fileUpload.DeleteImage(existingPromotion.ImagePath);
                    }

                    // Save new image and update ImagePath
                    existingPromotion.ImagePath = await fileUpload.SaveImage("images", formFile);
                }
                // If formFile is null, do nothing, which will keep the existing image

                // Update entity in DbContext
                _dbContext.Promotions.Update(existingPromotion);
                await _dbContext.SaveChangesAsync();

                // Prepare response DTO
                var updatedPromotionDTO = new PromotionDTO
                {
                    Id = existingPromotion.Id,
                    Name = existingPromotion.Name,
                    Description = existingPromotion.Description,
                    ImagePath = existingPromotion.ImagePath,
                    Status = existingPromotion.Status,
                    QuantityTable = existingPromotion.QuantityTable,
                    Price = existingPromotion.Price
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
                    Message = "Error updating promotion: " + ex.Message,
                    Data = null
                });
            }
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeletePromotion(int id)
        {
            var fileUpload = new FileUpload(_webHostEnvironment);

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
                    fileUpload.DeleteImage(promotionToDelete.ImagePath);
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
