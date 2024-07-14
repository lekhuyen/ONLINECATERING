using APIRESPONSE.Models;
using INFORMATIONAPI.Models;
using INFORMATIONAPI.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace INFORMATIONAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutController : ControllerBase
    {
        private readonly IAboutRepositories _aboutRepository;
        private readonly IWebHostEnvironment _env; // For file handling

        public AboutController(IAboutRepositories aboutRepository, IWebHostEnvironment env)
        {
            _aboutRepository = aboutRepository;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContent()
        {
            try
            {
                var aboutContent = await _aboutRepository.GetAllAsync();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get aboutContent Successfully",
                    Data = aboutContent
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneById(string id)
        {
            try
            {
                var about = await _aboutRepository.GetByIdAsync(id);
                if (about == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "About not found",
                        Data = about
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Update About Successfully",
                    Data = about
                });  // Ensure 'Id' property is correctly populated in 'about'
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

        [HttpPost]
        public async Task<IActionResult> CreateContent([FromForm] About abt, IFormFile? imageFile)
        {
            try
            {
                await _aboutRepository.CreateAsync(abt, imageFile);
                return Created("success", new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Add About Us Successfully",
                    Data = abt
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Create content failed"
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, [FromForm] About abt, IFormFile? imageFile)
        {
            try
            {
                // Ensure the Id in the about object matches the id parameter
                if (abt.Id != id)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Mismatched ID in about object and parameter",
                        Data = null
                    });

                }

                var updated = await _aboutRepository.UpdateAsync(id, abt, imageFile);
                if (!updated)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "About Us not found",
                    });
                }

                // Retrieve the updated About object after update
                var updatedAbout = await _aboutRepository.GetByIdAsync(id);

                // Ensure the response format meets your requirements
                var response = new
                {
                    id = updatedAbout.Id,
                    title = updatedAbout.Title,
                    content = updatedAbout.Content,
                    imagePath = updatedAbout.ImagePath ?? ""  // Ensure imagePath is not null
                };

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Update restaurant Successfully",
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var deleted = await _aboutRepository.DeleteAsync(id);
                if (!deleted)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "About Us is not found",
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Delete about us successfully",
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
    }
}
