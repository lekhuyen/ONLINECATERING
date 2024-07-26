using APIRESPONSE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using REDISCLIENT;
using RESTAURANT.API.DTOs;
using RESTAURANT.API.Helpers;
using RESTAURANT.API.Repositories;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenu _menu;
        private readonly RedisClient _redisClient;
        public MenuController(IMenu menu, RedisClient redisClient)
        {
            _menu = menu;
            _redisClient = redisClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMenu()
        {
            var menus = await _menu.GetAllMenu();
            return Ok(menus);
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
        public async Task<IActionResult> AddMenu([FromForm] CreateMenuDTO menuDTO, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(formFile != null)
                    {
                        //var imagePath = await FileUpload.SaveImage("images", formFile);
                        //menuDTO.MenuImage = imagePath;
                    }
                    
                    await _menu.AddMenu(menuDTO);
                   


                    //reids
                    var menuJson = JsonConvert.SerializeObject(menuDTO);
                    _redisClient.Publish("created_menu", menuJson);

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
        public async Task<IActionResult> UpdateMenu( int id, [FromForm] EditMenuDTO menuDTO, IFormFile formFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var menu = await _menu.GetMenuById(id);
                    if (menu != null)
                    {
                        if (formFile != null)
                        {
                            //var imagePath = await FileUpload.SaveImage("images", formFile);
                            //if(menu.MenuImage != null)
                            //{
                            //    FileUpload.DeleteImage(menu.MenuImage);
                            //}

                            //menu.MenuImage = imagePath;
                        }

                        menu.MenuName = menuDTO.MenuName;
                        menu.RestaurantId = menuDTO.RestaurantId;
                        menu.Price = menuDTO.Price;
                        menu.Ingredient = menuDTO.Ingredient;

                        await  _menu.UpdateMenu(menu);

                        //reids
                        menuDTO.Id = id;
                        var menuJson = JsonConvert.SerializeObject(menuDTO);
                        _redisClient.Publish("update_menu", menuJson);

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
