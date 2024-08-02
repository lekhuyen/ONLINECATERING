using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using APIRESPONSE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.Helpers;
using RESTAURANT.API.Models;
using RESTAURANT.API.Repositories;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LobbyController : ControllerBase
    {
        private readonly ILobbyRepository _lobbyRepository;
        private readonly DatabaseContext _dbContext;
        private readonly FileUpload _fileUpload;

        public LobbyController(ILobbyRepository lobbyRepository, DatabaseContext dbContext, FileUpload fileUpload)
        {
            _lobbyRepository = lobbyRepository;
            _dbContext = dbContext;
            _fileUpload = fileUpload;
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
                    Message = "Error fetching lobbies",
                    Data = ex.Message
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
                    Message = "Error fetching lobby",
                    Data = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] Lobby lobby, [FromForm] List<IFormFile> files)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Invalid model state",
                        Data = ModelState
                    });
                }

                var uploadedImages = new List<LobbyImages>();

                // Save images using FileUpload class
                foreach (var file in files)
                {
                    var imageUrl = await _fileUpload.SaveImage("images", file); // 'images' is the subfolder name
                    var lobbyImage = new LobbyImages { ImagesUrl = imageUrl };
                    uploadedImages.Add(lobbyImage);
                }

                // Assign uploaded images to the lobby
                var createdLobby = await _lobbyRepository.CreateLobby(lobby, uploadedImages);

                return Created("success", new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Create lobby successfully",
                    Data = createdLobby
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Error creating lobby",
                    Data = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] Lobby lobby, [FromForm] List<IFormFile> files)
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

                // Handle existing images
                var existingImages = existingLobby.LobbyImages.ToList();

                // If no files are uploaded, retain existing images
                if (files == null || files.Count == 0)
                {
                    lobby.LobbyImages = existingImages;
                }
                else
                {
                    // Delete existing images associated with the lobby
                    foreach (var image in existingImages)
                    {
                        _fileUpload.DeleteImage(image.ImagesUrl);
                    }

                    // Save new images
                    var uploadedImages = new List<LobbyImages>();
                    foreach (var file in files)
                    {
                        var imageUrl = await _fileUpload.SaveImage("images", file); // 'images' is the subfolder name
                        var lobbyImage = new LobbyImages { ImagesUrl = imageUrl };
                        uploadedImages.Add(lobbyImage);
                    }

                    // Update lobby with new images
                    lobby.LobbyImages = uploadedImages;
                }

                // Update other details of the lobby
                existingLobby.LobbyName = lobby.LobbyName;
                existingLobby.Description = lobby.Description;
                existingLobby.Area = lobby.Area;
                existingLobby.Type = lobby.Type;
                existingLobby.Price = lobby.Price;

                // Save changes to the database
                var updatedLobby = await _lobbyRepository.UpdateLobby(id, existingLobby, lobby.LobbyImages);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Update lobby successfully",
                    Data = updatedLobby
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Error updating lobby",
                    Data = ex.Message
                });
            }
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLobby(int id)
        {
            try
            {
                var lobby = await _dbContext.Lobbies
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

                // Delete images associated with the lobby
                foreach (var image in lobby.LobbyImages)
                {
                    _fileUpload.DeleteImage(image.ImagesUrl);
                }

                // Remove lobby and associated images from database
                _dbContext.LobbiesImages.RemoveRange(lobby.LobbyImages);
                _dbContext.Lobbies.Remove(lobby);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Delete lobby and images successfully",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Error deleting lobby",
                    Data = ex.Message
                });
            }
        }
    }
}
