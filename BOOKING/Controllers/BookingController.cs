using APIRESPONSE.Models;
using BOOKING.API.DTOs;
using BOOKING.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using REDISCLIENT;

namespace BOOKING.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly RedisClient _redisClient;
        private readonly DatabaseContext _dbContext;
        public BookingController(DatabaseContext dbContext, RedisClient redisClient)
        {
            _dbContext = dbContext;
            _redisClient = redisClient;
        }
        [HttpPost]
        public async Task<IActionResult> AddBooking(Booking booking)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var booked = await _dbContext.Bookings
                            .FirstOrDefaultAsync(u => u.UserId == booking.UserId && u.RestaurantId == booking.RestaurantId);

                    if(booked != null)
                    {
                        return BadRequest(new ApiResponse
                        {
                            Success = false,
                            Status = 1,
                            Message = "You were booked",
                        });
                    }

                    await _dbContext.Bookings.AddAsync(booking);
                    await _dbContext.SaveChangesAsync();

                    //redis

                    var bookingDTO = new BookingDTO
                    {
                        UserId = booking.UserId,
                        RestaurantId = booking.RestaurantId,
                        MenuId = booking.MenuId,
                        DayArrive = booking.DayArrive,
                        Hour = booking.Hour,
                        Status = booking.Status,
                        Member = booking.Member,
                        Pont = booking.Pont,
                        Total = booking.Total,
                        Description = booking.Description,
                    };

                    var bookingJson = JsonConvert.SerializeObject(bookingDTO);
                    _redisClient.Publish("create-booking", bookingJson);
                    //--------------------------------

                    return Created("success", new ApiResponse
                    {
                        Success = true,
                        Status = 0,
                        Message = "Booking successful"
                    });
                }
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Booking failed"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Create Users failed"
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBooking()
        {
            try
            {
                var booking = await _dbContext.Bookings.ToListAsync();
                await _dbContext.SaveChangesAsync();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get successful",
                    Data = booking
                });
               
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Server something wrong"
                });
            }
        }
        [HttpPut("{userId}/{restaurantId}")]
        public async Task<IActionResult> UpdateBooing(int restaurantId, int userId, Booking bookingEdit)
        {
            try
            {
                var booking = await _dbContext.Bookings
                    .FirstOrDefaultAsync(u => u.UserId == userId && u.RestaurantId == restaurantId);
                if (booking == null)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Booking not found"
                    });
                }
                //UserId = bookingEdit.UserId,
                //RestaurantId = bookingEdit.RestaurantId,
                booking.MenuId = bookingEdit.MenuId;
                booking.DayArrive = bookingEdit.DayArrive;
                booking.Hour = bookingEdit.Hour;
                booking.Status = bookingEdit.Status;
                booking.Member = bookingEdit.Member;
                booking.Pont = bookingEdit.Pont;
                booking.Total = bookingEdit.Total;
                booking.Description = bookingEdit.Description;

                _dbContext.Bookings.Update(booking);
                await _dbContext.SaveChangesAsync();

                var bookingDTO = new BookingDTO
                {
                    UserId = bookingEdit.UserId,
                    RestaurantId = bookingEdit.RestaurantId,
                    MenuId = bookingEdit.MenuId,
                    DayArrive = bookingEdit.DayArrive,
                    Hour = bookingEdit.Hour,
                    Status = bookingEdit.Status,
                    Member = bookingEdit.Member,
                    Pont = bookingEdit.Pont,
                    Total = bookingEdit.Total,
                    Description = bookingEdit.Description,
                };

                //redis
                var bookingJson = JsonConvert.SerializeObject(bookingDTO);
                _redisClient.Publish("edit-booking", bookingJson);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Update successful",
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Server something wrong"
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _dbContext.Bookings.FindAsync(id);
            if(booking == null)
            {
                 return NotFound();
            }
            _dbContext.Bookings.Remove(booking);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
