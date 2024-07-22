using System;
using System.Threading.Tasks;
using APIRESPONSE.Models;
using Microsoft.AspNetCore.Mvc;
using RESTAURANT.API.Models;
using RESTAURANT.API.Repositories;

namespace RESTAURANT.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LobbyController : ControllerBase
	{
		private readonly ILobbyRepository _lobbyRepository;

		public LobbyController(ILobbyRepository lobbyRepository)
		{
			_lobbyRepository = lobbyRepository;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				var lobbies = await _lobbyRepository.GetAllLobbies();
				return Ok(new ApiResponse
				{
					Success = true,
					Status = 0,
					Message = "Get all lobbies successfully",
					Data = lobbies
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
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
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

				await _lobbyRepository.DeleteLobby(id);

				return Ok(new ApiResponse
				{
					Success = true,
					Status = 0,
					Message = "Delete lobby successfully",
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
	}
}
