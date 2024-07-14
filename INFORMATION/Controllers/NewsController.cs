using APIRESPONSE.Models;
using INFORMATIONAPI.Models;
using INFORMATIONAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace INFORMATIONAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsRepositories _newsRepositories;
        private readonly IWebHostEnvironment _env; // For file handling

        public NewsController(INewsRepositories newsRepositories, IWebHostEnvironment env)
        {
            _newsRepositories = newsRepositories;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContent()
        {
            try
            {
                var newsContent = await _newsRepositories.GetAllAsync();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get aboutContent Successfully",
                    Data = newsContent
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
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Update News Successfully",
                    Data = news
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
        public async Task<IActionResult> CreateContent([FromForm] News news, IFormFile? imageFile)
        {
            try
            {
                await _newsRepositories.CreateAsync(news, imageFile);
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
                    Message = "Create content failed"
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(string id, [FromForm] News news, IFormFile? imageFile)
        {
            try
            {

                // Ensure the Id in the about object matches the id parameter
                if (news.Id != id)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Mismatched ID in about object and parameter",
                        Data = null
                    });
                }

                var updated = await _newsRepositories.UpdateAsync(id, news, imageFile);
                if (!updated)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "News not found",
                    });
                }

                // Retrieve the updated About object after update
                var updatedNews = await _newsRepositories.GetByIdAsync(id);

                // Ensure the response format meets your requirements
                var response = new
                {
                    id = updatedNews.Id,
                    title = updatedNews.Title,
                    content = updatedNews.Content,
                    imagePath = updatedNews.ImagePath ?? ""  // Ensure imagePath is not null
                };

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Update News Successfully",
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
    }
}
