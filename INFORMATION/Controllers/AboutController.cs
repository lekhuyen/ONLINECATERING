using APIRESPONSE.Models;
using INFORMATION.API.Models;
using INFORMATIONAPI.Models;
using INFORMATIONAPI.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace INFORMATIONAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutController : ControllerBase
    {
        private readonly IAboutRepositories _aboutRepository;
        private readonly IWebHostEnvironment _env;

        public AboutController(IAboutRepositories aboutRepository, IWebHostEnvironment env)
        {
            _aboutRepository = aboutRepository;
            _env = env ?? throw new ArgumentNullException(nameof(env));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContent()
        {
            try
            {
                var aboutContent = await _aboutRepository.GetAllAsync();

                // Retrieve all about types to map ids to names
                var AboutTypes = await _aboutRepository.GetAllAboutTypesAsync();
                var AboutTypeMap = AboutTypes.ToDictionary(nt => nt.Id, nt => nt.AboutTypeName);

                var formattedAbout = aboutContent.Select(about => new
                {
                    id = about.Id,
                    title = about.Title,
                    content = about.Content,
                    AboutTypeName = AboutTypeMap.ContainsKey(about.AboutTypeId) ? AboutTypeMap[about.AboutTypeId] : "",
                    imagePaths = about.ImagePaths != null && about.ImagePaths.Count > 0 ? about.ImagePaths : new List<string>()
                });

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get About Content Successfully",
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
                        Message = "About Content not found",
                        Data = about
                    });
                }

                // Retrieve all about types to map ids to names
                var AboutType = await _aboutRepository.GetAboutTypeByIdAsync(about.AboutTypeId);
                string aboutTypeName = AboutType != null ? AboutType.AboutTypeName : "";

                // Ensure imagePath is not null
                var imagePaths = about.ImagePaths != null && about.ImagePaths.Count > 0 ? about.ImagePaths : new List<string>();
                var formattedAbout = new
                {
                    id = about.Id,
                    title = about.Title,
                    content = about.Content,
                    aboutTypeName = aboutTypeName,
                    imagePaths = about.ImagePaths != null && about.ImagePaths.Count > 0 ? about.ImagePaths : new List<string>()
                };


                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "About Content founded Successfully",
                    Data = about
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

        [HttpPost]
        public async Task<IActionResult> CreateContent([FromForm] About abt, [FromForm] List<IFormFile>? imageFiles)
        {
            try
            {
                if (imageFiles != null && imageFiles.Count > 5)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "You can upload up to 5 images."
                    });
                }

                await _aboutRepository.CreateAsync(abt, imageFiles);
                return Created("success", new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Add About Content Successfully",
                    Data = abt
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Create About Content failed"
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, [FromForm] About abt, List<IFormFile>? imageFiles)
        {
            try
            {
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

                // Check if imageFiles contains more than 5 files
                if (imageFiles != null && imageFiles.Count > 5)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "You can upload up to 5 images."
                    });
                }

                // Update with new data and image files
                var updated = await _aboutRepository.UpdateAsync(id, abt, imageFiles);
                if (!updated)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "About Content not found",
                    });
                }

                // Retrieve the updated About object after update
                var updatedAbout = await _aboutRepository.GetByIdAsync(id);

                // Retrieve all about types to map ids to names
                var AboutType = await _aboutRepository.GetAboutTypeByIdAsync(updatedAbout.AboutTypeId);
                string aboutTypeName = AboutType != null ? AboutType.AboutTypeName : "";

                // Prepare response data
                var response = new
                {
                    id = updatedAbout.Id,
                    title = updatedAbout.Title,
                    content = updatedAbout.Content,
                    newsTypeName = aboutTypeName,
                    imagePaths = updatedAbout.ImagePaths ?? new List<string>()
                };

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Update About Content Successfully",
                    Data = response
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
                        Message = "About Content is not found",
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Delete about content successfully",
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

        // About Types

        [HttpGet("newtypes")]
        public async Task<IActionResult> GetAllAboutTypes()
        {
            try
            {
                var aboutTypes = await _aboutRepository.GetAllAboutTypesAsync();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get All About Types Successfully",
                    Data = aboutTypes
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

        [HttpGet("abouttypes/{id}")]
        public async Task<IActionResult> GetNewTypeById(string id)
        {
            try
            {
                var aboutType = await _aboutRepository.GetAboutTypeByIdAsync(id);
                if (aboutType == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "About Type not found",
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get About Type Successfully",
                    Data = aboutType
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

        [HttpPost("abouttypes")]
        public async Task<IActionResult> CreateAboutType([FromBody] AboutType aboutType)
        {
            try
            {
                // Check if a AboutType with the same name already exists
                var existingType = await _aboutRepository.GetAboutTypeByNameAsync(aboutType.AboutTypeName);
                if (existingType != null)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "A AboutType with the same name already exists",
                        Data = null
                    });
                }

                await _aboutRepository.CreateAboutTypeAsync(aboutType);
                return Created("success", new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Add About Type Successfully",
                    Data = aboutType
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Create About Type failed",
                    Data = null
                });
            }
        }

        [HttpPut("abouttypes/{id}")]
        public async Task<IActionResult> EditAboutType(string id, [FromBody] AboutType aboutType)
        {
            try
            {
                // Ensure the Id in the news type object matches the id parameter
                if (aboutType.Id != id)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Mismatched ID in about type object and parameter",
                        Data = null
                    });
                }

                // Check if a NewsType with the same name already exists (excluding the current type being updated)
                var existingType = await _aboutRepository.GetAboutTypeByNameAsync(aboutType.AboutTypeName);
                if (existingType != null && existingType.Id != id)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "A About Type with the same name already exists",
                        Data = null
                    });
                }

                // Update the news type
                var updated = await _aboutRepository.UpdateAboutTypeAsync(id, aboutType);
                if (!updated)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "About Type not found",
                    });
                }

                // Retrieve the updated AboutType object after update
                var updatedAboutType = await _aboutRepository.GetAboutTypeByIdAsync(id);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Update About Type Successfully",
                    Data = updatedAboutType
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = $"Error from service: {ex.Message}",
                    Data = null
                });
            }
        }


        [HttpDelete("abouttypes/{id}")]
        public async Task<IActionResult> DeleteAboutType(string id)
        {
            try
            {
                var deleted = await _aboutRepository.DeleteAboutTypeAsync(id);
                if (!deleted)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "About Type not found",
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Delete About Type Successfully",
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
