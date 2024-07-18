using APIRESPONSE.Models;
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
    public class NewsController : ControllerBase
    {
        private readonly INewsRepositories _newsRepositories;
        private readonly IWebHostEnvironment _env;

        public NewsController(INewsRepositories newsRepositories, IWebHostEnvironment env)
        {
            _newsRepositories = newsRepositories ?? throw new ArgumentNullException(nameof(newsRepositories));
            _env = env ?? throw new ArgumentNullException(nameof(env));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContent()
        {
            try
            {
                var newsContent = await _newsRepositories.GetAllAsync();

                // Retrieve all news types to map ids to names
                var newsTypes = await _newsRepositories.GetAllNewTypesAsync();
                var newsTypeMap = newsTypes.ToDictionary(nt => nt.Id, nt => nt.NewsTypeName);

                // Map news type names to news objects
                var formattedNews = newsContent.Select(news => new
                {
                    id = news.Id,
                    title = news.Title,
                    content = news.Content,
                    newsTypeName = newsTypeMap.ContainsKey(news.NewsTypeId) ? newsTypeMap[news.NewsTypeId] : "",
                    imagePaths = news.ImagePaths != null && news.ImagePaths.Count > 0 ? news.ImagePaths : new List<string>()
                });

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get News Successfully",
                    Data = formattedNews.ToList() // Convert to list if necessary
                });
            }
            catch (Exception ex)
            {
                string errorMessage = "Error fetching all news content: " + ex.Message;
                Console.WriteLine(errorMessage); // Output to console for debugging purposes
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = errorMessage,
                    Data = null
                });
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneById(string id)
        {
            try
            {
                var news = await _newsRepositories.GetByIdAsync(id);
                if (news == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "News not found",
                    });
                }

                // Retrieve the news type name for the current news object
                var newsType = await _newsRepositories.GetNewTypeByIdAsync(news.NewsTypeId);
                string newsTypeName = newsType != null ? newsType.NewsTypeName : "";

                // Ensure imagePath is not null
                var imagePaths = news.ImagePaths != null && news.ImagePaths.Count > 0 ? news.ImagePaths : new List<string>();

                var formattedNews = new
                {
                    id = news.Id,
                    title = news.Title,
                    content = news.Content,
                    newsTypeName = newsTypeName,
                    imagePaths = news.ImagePaths != null && news.ImagePaths.Count > 0 ? news.ImagePaths : new List<string>()
                };

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get News Successfully",
                    Data = formattedNews
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
        public async Task<IActionResult> CreateContent([FromForm] News news, List<IFormFile>? imageFiles)
        {
            try
            {
                await _newsRepositories.CreateAsync(news, imageFiles);
                return Created("success", new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Add News Successfully",
                    Data = news
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Create news failed"
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, [FromForm] News news, List<IFormFile>? imageFiles)
        {
            try
            {
                // Ensure the Id in the news object matches the id parameter
                if (news.Id != id)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Mismatched ID in news object and parameter",
                        Data = null
                    });
                }

                var updated = await _newsRepositories.UpdateAsync(id, news, imageFiles);
                if (!updated)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "News not found",
                    });
                }

                if (imageFiles != null && imageFiles.Count > 5)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "You can upload up to 5 images."
                    });
                }

                // Retrieve the updated News object after update
                var updatedNews = await _newsRepositories.GetByIdAsync(id);

                // Retrieve the news type name for the updated news object
                var newsType = await _newsRepositories.GetNewTypeByIdAsync(updatedNews.NewsTypeId);
                string newsTypeName = newsType != null ? newsType.NewsTypeName : "";


                var response = new
                {
                    id = updatedNews.Id,
                    title = updatedNews.Title,
                    content = updatedNews.Content,
                    newsTypeName = newsTypeName,
                    imagePaths = updatedNews.ImagePaths ?? new List<string>()
                };

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Update News Successfully",
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
                var deleted = await _newsRepositories.DeleteAsync(id);
                if (!deleted)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "News not found",
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Delete News Successfully",
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

        // News Types

        [HttpGet("newtypes")]
        public async Task<IActionResult> GetAllNewTypes()
        {
            try
            {
                var newTypes = await _newsRepositories.GetAllNewTypesAsync();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get All News Types Successfully",
                    Data = newTypes
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

        [HttpGet("newtypes/{id}")]
        public async Task<IActionResult> GetNewTypeById(string id)
        {
            try
            {
                var newType = await _newsRepositories.GetNewTypeByIdAsync(id);
                if (newType == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "News Type not found",
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get News Type Successfully",
                    Data = newType
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

        [HttpPost("newtypes")]
        public async Task<IActionResult> CreateNewType([FromBody] NewsType newType)
        {
            try
            {
                // Check if a NewsType with the same name already exists
                var existingType = await _newsRepositories.GetNewsTypeByNameAsync(newType.NewsTypeName);
                if (existingType != null)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "A NewsType with the same name already exists",
                        Data = null
                    });
                }

                await _newsRepositories.CreateNewTypeAsync(newType);
                return Created("success", new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Add News Type Successfully",
                    Data = newType
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Create News Type failed",
                    Data = null
                });
            }
        }

        [HttpPut("newtypes/{id}")]
        public async Task<IActionResult> EditNewType(string id, [FromBody] NewsType newType)
        {
            try
            {
                // Ensure the Id in the news type object matches the id parameter
                if (newType.Id != id)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Mismatched ID in news type object and parameter",
                        Data = null
                    });
                }

                // Check if a NewsType with the same name already exists (excluding the current type being updated)
                var existingType = await _newsRepositories.GetNewsTypeByNameAsync(newType.NewsTypeName);
                if (existingType != null && existingType.Id != id)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "A NewsType with the same name already exists",
                        Data = null
                    });
                }

                // Update the news type
                var updated = await _newsRepositories.UpdateNewTypeAsync(id, newType);
                if (!updated)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "News Type not found",
                    });
                }

                // Retrieve the updated NewsType object after update
                var updatedNewsType = await _newsRepositories.GetNewTypeByIdAsync(id);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Update News Type Successfully",
                    Data = updatedNewsType
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


        [HttpDelete("newtypes/{id}")]
        public async Task<IActionResult> DeleteNewType(string id)
        {
            try
            {
                var deleted = await _newsRepositories.DeleteNewTypeAsync(id);
                if (!deleted)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "News Type not found",
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Delete News Type Successfully",
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
