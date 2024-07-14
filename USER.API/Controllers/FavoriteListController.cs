using APIRESPONSE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using USER.API.DTOs;
using USER.API.Models;
using USER.API.Repositories;

namespace USER.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteListController : ControllerBase
    {
        private readonly IFavoriteList _favoriteList;
        public FavoriteListController(IFavoriteList favorite)
        {
            _favoriteList = favorite;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var favorites = await _favoriteList.GetFavoriteListAsync();
                
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get favorites Successfully",
                    Data = favorites
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
        public async Task<IActionResult> Add(FavoriteListDTO favorite)
        {
            try
            {
                await _favoriteList.AddFavorite(favorite);
                return Created("success", new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Add favorite successfully"
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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _favoriteList.DeleteFavorite(id);
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Delete favorite successfully"
                });
            }
            catch(Exception ex)
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
