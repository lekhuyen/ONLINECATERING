using APIRESPONSE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using REDISCLIENT;
using RESTAURANT.API.DTOs;
using RESTAURANT.API.Models;
using RESTAURANT.API.Repositories;
using System.Net;
using System.Runtime.InteropServices;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurant _irestaurannt;
        private readonly RedisClient _redisClient;
        public RestaurantController(IRestaurant irestaurannt, RedisClient redisClient)
        {
            _irestaurannt = irestaurannt;
            _redisClient = redisClient;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var restaurants = await _irestaurannt.GetRestaurantsAsync();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get restaurant Successfully",
                    Data = restaurants
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
        public async Task<IActionResult> AddRestaurant(PostRestaurantDTO restaurant)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var res = await _irestaurannt.AddRestaurantAsync(restaurant);

                    return Created("success", new ApiResponse
                    {
                        Success = true,
                        Status = 0,
                        Message = "Add restaurant Successfully",
                        Data = res
                    });
                }
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Create restaurant failed"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Create restaurant failed"
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PostRestaurantDTO restaurant)
        {
            try
            {
                var res = await _irestaurannt.GetRestaurantByIdAsync(id);
                if(res != null)
                {
                    if (ModelState.IsValid)
                    {
                        res.RestaurantName = restaurant.RestaurantName;
                        res.City = restaurant.City;
                        res.Address = restaurant.Address;
                        res.Open = restaurant.Open;
                        res.Close = restaurant.Close;
                        res.CategoryId = (int)restaurant.CategoryId;
                        res.UserId = (int)res.UserId;

                        await _irestaurannt.UpdateRestaurantAsync(res);
                        return Ok(new ApiResponse
                        {
                            Success = true,
                            Status = 0,
                            Message = "Update restaurant Successfully",
                        });
                    }
                }
                
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Restaurant not found",
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) 
        {
            try
            {
                var resExisted = await _irestaurannt.GetRestaurantByIdAsync(id);
                if (resExisted != null)
                {
                    await _irestaurannt.DeleteRestaurantAsync(id);
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Status = 0,
                        Message = "Delete restaurant successfully",
                    });
                }
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Restaurant is not found",
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRestaurantById(int id)
        {
            try
            {
                var resExisted = await _irestaurannt.GetOneRestaurantByIdAsync(id);
                if (resExisted != null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Status = 0,
                        Message = "Get restaurant successfully",
                        Data = resExisted
                    });
                }
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Restaurant is not found",
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
