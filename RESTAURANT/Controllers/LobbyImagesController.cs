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

		public LobbyImagesController(DatabaseContext databaseContext, ILobbyImagesRepository lobbyImagesRepository)
		{
			_databaseContext = databaseContext;
			_lobbyImagesRepository = lobbyImagesRepository;
		}

		[HttpPost]
		public async Task<IActionResult> AddImages([FromForm] int lobbyId, List<IFormFile> formFiles)
		{
			try
			{
				if (formFiles.Count == 0)
				{
					return BadRequest(new ApiResponse
					{
						Success = false,
						Status = 1,
						Message = "No images selected",
					});
				}

				foreach (var item in formFiles)
				{
					var imagePath = await FileUpload.SaveImage("images", item);
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

		[HttpGet("{imageId}")]
		public async Task<IActionResult> GetImageById(int imageId)
		{
			try
			{
				var image = await _databaseContext.LobbiesImages.FirstOrDefaultAsync(r => r.Id == imageId);
				if (image != null)
				{
					return Ok(new ApiResponse
					{
						Success = true,
						Status = 0,
						Data = image
					});
				}
				return NotFound(new ApiResponse
				{
					Success = false,
					Status = 1,
					Message = "Image not found",
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
		public async Task<IActionResult> DeleteImage(int id)
		{
			try
			{
				var image = await _databaseContext.LobbiesImages.FirstOrDefaultAsync(r => r.Id == id);
				if (image == null)
				{
					return NotFound(new ApiResponse
					{
						Success = false,
						Status = 1,
						Message = "Image not found",
					});
				}

				FileUpload.DeleteImage(image.ImagesUrl);
				_databaseContext.Remove(image);
				await _databaseContext.SaveChangesAsync();

				return Ok(new ApiResponse
				{
					Success = true,
					Status = 0,
					Message = "Deleted",
				});
			}
			catch (Exception ex)
			{
				return BadRequest(new ApiResponse
				{
					Success = false,
					Status = 1,
					Message = "Error from service",
				});
			}
		}
	}
}
