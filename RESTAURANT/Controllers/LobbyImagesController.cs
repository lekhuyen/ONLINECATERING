using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIRESPONSE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.Helpers;
using RESTAURANT.API.Models;
using RESTAURANT.API.Repositories;

namespace RESTAURANT.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LobbyImagesController : ControllerBase
	{
		private readonly DatabaseContext _databaseContext;
		private readonly ILobbyImagesRepository _lobbyImagesRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public LobbyImagesController(DatabaseContext databaseContext, ILobbyImagesRepository lobbyImagesRepository, IWebHostEnvironment webHostEnvironment)
		{
			_databaseContext = databaseContext;
			_lobbyImagesRepository = lobbyImagesRepository;
			_webHostEnvironment = webHostEnvironment;
		}
		[HttpGet]
		public async Task<IActionResult> GetAllImages()
		{
			try
			{
				var images = await _databaseContext.LobbiesImages.ToListAsync();
				return Ok(images);
			}catch (Exception ex)
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
		public async Task<IActionResult> AddImages([FromForm] int lobbyId, List<IFormFile> formFiles)
		{
            var fileUpload = new FileUpload(_webHostEnvironment);

            try
            {
				if (formFiles == null || formFiles.Count == 0)
				{
					return BadRequest(new ApiResponse
					{
						Success = false,
						Status = 1,
						Message = "No images selected",
					});
				}

				var addedImages = new List<LobbyImages>();

				foreach (var item in formFiles)
                {
                    var imagePath = await fileUpload.SaveImage("images", item);
					var lobbyImage = new LobbyImages
					{
						LobbyId = lobbyId,
						ImagesUrl = imagePath,
					};
					await _lobbyImagesRepository.CreateLobbyImage(lobbyImage);
				}

				return Created("success", new ApiResponse
				{
					Success = true,
					Status = 0,
					Message = "Add images Successfully",
					Data = addedImages
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

		[HttpGet("images/{lobbyId}")]
		public async Task<IActionResult> GetImagesByLobbyId(int lobbyId)
		{
			try
			{
				var images = await _databaseContext.LobbiesImages
					.Where(li => li.LobbyId == lobbyId)
					.Select(li => li.ImagesUrl)
					.ToListAsync();

				if (images != null && images.Any())
				{
					return Ok(new ApiResponse
					{
						Success = true,
						Status = 0,
						Message = "Get images by lobbyId successfully",
						Data = images
					});
				}

				return NotFound(new ApiResponse
				{
					Success = false,
					Status = 1,
					Message = "No images found for the given lobbyId"
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

        [HttpDelete("{id}/images")]
        public async Task<IActionResult> DeleteLobbyImages(int id)
        {
            try
            {
                var lobby = await _databaseContext.Lobbies
                    .Include(l => l.LobbyImages)
                    .FirstOrDefaultAsync(l => l.Id == id);

                if (lobby == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Lobby not found"
                    });
                }

                // Delete all images associated with the lobby
                foreach (var image in lobby.LobbyImages)
                {

                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.ImagesUrl);

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    else
                    {
                        // Log a warning or handle accordingly if the file doesn't exist
                        Console.WriteLine($"File not found at path: {filePath}");
                    }
                }

                // Remove images from the database
                _databaseContext.LobbiesImages.RemoveRange(lobby.LobbyImages);
                await _databaseContext.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Delete lobby images successfully",
                    Data = lobby // Optionally return the lobby data after images are deleted
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = $"Error deleting lobby images: {ex.Message}",
                    Data = null
                });
            }
        }



    }
}

