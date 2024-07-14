using APIRESPONSE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTAURANT.API.DTOs;
using RESTAURANT.API.Repositories;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenu _menu;
        public MenuController(IMenu menu)
        {
            _menu = menu;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneMenu(int id)
        {
            try
            {
                var menu = await _menu.GetOneMenuById(id);
                if(menu != null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Status = 0,
                        Message = "Get menu Successfully",
                        Data = menu
                    });
                }
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Menu not found",
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
        public async Task<IActionResult> AddMenu(CreateMenuDTO menuDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _menu.AddMenu(menuDTO);
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Status = 0,
                        Message = "Create menu Successfully",
                    });
                }
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Empty",
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenu(int id, CreateMenuDTO menuDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var menu = await _menu.GetMenuById(id);
                    if (menu != null)
                    {
                        menu.MenuName = menuDTO.MenuName;
                        menu.RestaurantId = menuDTO.RestaurantId;
                        menu.Price = menuDTO.Price;
                        menu.Ingredient = menuDTO.Ingredient;

                        await  _menu.UpdateMenu(menu);

                        return Ok(new ApiResponse
                        {
                            Success = true,
                            Status = 0,
                            Message = "Update menu Successfully",
                        });
                    }
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Menu not found",
                    });
                }
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Empty",
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
        public async Task<IActionResult> DeleteMenu(int id)
        {
            try
            {
                var menu = await _menu.GetMenuById(id);
                if (menu != null)
                {
                    await _menu.DeleteMenu(id);
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Status = 0,
                        Message = "Delete menu Successfully",
                    });
                }
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Menu not found",
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
