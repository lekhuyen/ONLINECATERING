using System;
using System.Threading.Tasks;
using APIRESPONSE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.Models;
using RESTAURANT.API.Repositories;

namespace RESTAURANT.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LobbyController : ControllerBase
	{
		private readonly DatabaseContext _dbContext;
		private readonly ILobbyRepository _lobbyRepository;

		public LobbyController(ILobbyRepository lobbyRepository, DatabaseContext dbContext)
		{
			_lobbyRepository = lobbyRepository;
			_dbContext = dbContext;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				var lobbies = await _lobbyRepository.GetAllLobbies();

                var responseData = lobbies.Select(lobby => new
                {
                    lobby.Id,
                    lobby.LobbyName,
                    lobby.Description,
                    lobby.Area,
                    lobby.Type,
					lobby.Price,
                    LobbyImages = lobby.LobbyImages?.Select(image => new
                    {
                        image.Id,
                        image.ImagesUrl
                    }).ToList()
                }).ToList();

                return Ok(new ApiResponse
				{
					Success = true,
					Status = 0,
					Message = "Get all lobbies successfully",
					Data = responseData
				});
			}
			catch (Exception ex)
			{
				return BadRequest(new ApiResponse
				{
					Success = false,
					Status = 1,
					Message = "Error from service"
				});
			}
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			try
			{
				var lobby = await _lobbyRepository.GetLobbyById(id);
				if (lobby == null)
				{
					return NotFound(new ApiResponse
					{
						Success = false,
						Status = 1,
						Message = "Lobby not found"
					});
				}
				return Ok(new ApiResponse
				{
					Success = true,
					Status = 0,
					Message = "Get lobby successfully",
					Data = lobby
				});
			}
			catch (Exception ex)
			{
				return BadRequest(new ApiResponse
				{
					Success = false,
					Status = 1,
					Message = "Error from service"
				});
			}
		}

		[HttpPost]
		public async Task<IActionResult> Add([FromBody] Lobby lobby)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(new ApiResponse
					{
						Success = false,
						Status = 1,
						Message = "Invalid model state"
					});
				}

				await _lobbyRepository.CreateLobby(lobby);

				return Created("success", new ApiResponse
				{
					Success = true,
					Status = 0,
					Message = "Create lobby successfully",
					Data = lobby
				});
			}
			catch (Exception ex)
			{
				return BadRequest(new ApiResponse
				{
					Success = false,
					Status = 1,
					Message = "Error from service"
				});
			}
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] Lobby lobby)
		{
			try
			{
				if (id != lobby.Id)
				{
					return BadRequest(new ApiResponse
					{
						Success = false,
						Status = 1,
						Message = "The ID is mismatched"
					});
				}
				var existingLobby = await _lobbyRepository.GetLobbyById(id);
				if (existingLobby == null)
				{
					return NotFound(new ApiResponse
					{
						Success = false,
						Status = 1,
						Message = "Lobby not found"
					});
				}

				existingLobby.LobbyName = lobby.LobbyName;
				existingLobby.Description = lobby.Description;
				existingLobby.Area = lobby.Area;
				existingLobby.Type = lobby.Type;

				await _lobbyRepository.UpdateLobby(existingLobby);

				return Ok(new ApiResponse
				{
					Success = true,
					Status = 0,
					Message = "Update lobby successfully",
					Data = existingLobby
				});
			}
			catch (Exception ex)
			{
				return BadRequest(new ApiResponse
				{
					Success = false,
					Status = 1,
					Message = "Error from service"
				});
			}
		}

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLobby(int id)
        {
            try
            {
                var lobby = await _dbContext.Lobbies
                    .Include(l => l.LobbyImages) // Ensure LobbyImages are included
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

                // Check if LobbyImages collection is not null
                if (lobby.LobbyImages != null)
                {
                    // Delete all images associated with the lobby from file system and database
                    foreach (var image in lobby.LobbyImages)
                    {
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.ImagesUrl);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }

                    // Remove images from the database
                    _dbContext.LobbiesImages.RemoveRange(lobby.LobbyImages);
                }

                // Remove the lobby itself
                _dbContext.Lobbies.Remove(lobby);

                // Save changes
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Delete lobby and images successfully",
                    Data = lobby // Optionally return deleted lobby data if needed
                });
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                Console.WriteLine($"Error deleting lobby with id {id}: {ex.Message}");
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Error deleting lobby",
                    Data = null
                });
            }
        }


    }
}
