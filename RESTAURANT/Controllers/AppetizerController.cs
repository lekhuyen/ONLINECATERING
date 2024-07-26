﻿using APIRESPONSE.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.DTOs;
using RESTAURANT.API.Helpers;
using RESTAURANT.API.Models;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppetizerController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AppetizerController(DatabaseContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;

        }
        [HttpGet]
        public async Task<IActionResult> GetAllAppetizer()
        {
            try
            {
                var appetizers = await _dbContext.Appetizers.ToListAsync();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get appetizers successfully",
                    Data = appetizers
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppetizerById(int id)
        {
            try
            {
                // Retrieve the appetizer with the specified id
                var appetizer = await _dbContext.Appetizers.FindAsync(id);

                if (appetizer == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Appetizer not found",
                        Data = null
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Appetizer retrieved successfully",
                    Data = appetizer
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

        [HttpPost]
        public async Task<IActionResult> AddAppetizer([FromForm] Appetizer appetizer, IFormFile formFile)
        {
            var fileUpload = new FileUpload(_webHostEnvironment);

            try
            {
                if (ModelState.IsValid)
                {
                    string result = null;
                    if (formFile != null)
                    {
                        result = await fileUpload.SaveImage("images", formFile);
                        //var imagePath = await FileUpload.SaveImage("images", formFile);
                        appetizer.AppetizerImage = result;
                    }

                    await _dbContext.Appetizers.AddAsync(appetizer);
                    await _dbContext.SaveChangesAsync();

                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Status = 0,
                        Message = "Create appetizer Successfully",
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
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppetizer(int id, [FromForm] Appetizer updatedAppetizer, IFormFile? formFile)
        {
            var fileUpload = new FileUpload(_webHostEnvironment);

            try
            {
                // Find the existing appetizer in the database
                var existingAppetizer = await _dbContext.Appetizers.FindAsync(id);

                if (existingAppetizer == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Appetizer not found",
                        Data = null
                    });
                }

                if (id != existingAppetizer.Id)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Mismatched ID in object and parameter",
                        Data = null
                    });
                }

                // Update the properties of the existing appetizer with the new values
                existingAppetizer.AppetizerName = updatedAppetizer.AppetizerName;
                existingAppetizer.Price = updatedAppetizer.Price;
                existingAppetizer.Quantity = updatedAppetizer.Quantity;

                // Handle image update
                if (formFile != null)
                {
                    // Delete old image if exists
                    if (!string.IsNullOrEmpty(existingAppetizer.AppetizerImage))
                    {
                        fileUpload.DeleteImage(existingAppetizer.AppetizerImage);
                    }

                    // Save the new image
                    string newImagePath = await fileUpload.SaveImage("images", formFile);
                    existingAppetizer.AppetizerImage = newImagePath;
                }


                // Save changes to the database
                _dbContext.Appetizers.Update(existingAppetizer);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Appetizer updated successfully",
                    Data = existingAppetizer
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppetizer(int id)
        {
            var fileUpload = new FileUpload(_webHostEnvironment);
            try
            {
                var appetizer = await _dbContext.Appetizers.FindAsync(id);

                if (appetizer == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Appetizer not found",
                    });
                }


                if (!string.IsNullOrEmpty(appetizer.AppetizerImage))
                {
                    fileUpload.DeleteImage(appetizer.AppetizerImage);
                }


                _dbContext.Appetizers.Remove(appetizer);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Appetizer deleted successfully"
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
