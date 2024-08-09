using APIRESPONSE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USER.API.DTOs;
using USER.API.Models;

namespace USER.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {

        private readonly DatabaseContext _dbContext;
        public RoomController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetRooms()
        {
            var rooms = await _dbContext.Rooms
                .Include(c => c.Users)
                .ToListAsync();
            var roomsDTO = rooms.Select(c => new RoomDTO
            {
                Id= c.Id,
                RoomCode= c.RoomCode,
                Users = c.Users.Select(u => new UserDTO
                {
                    Id = u.Id,
                    UserEmail = u.UserEmail,
                    UserName = u.UserName,
                }).ToList(),
            }).ToList();
            if(rooms == null)
            {
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 1,
                    Message = "Room k co",
                });
            }
            return Ok(new ApiResponse
            {
                Success = true,
                Status = 0,
                Message = "Get room Successfully",
                Data = roomsDTO
            });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var room = await _dbContext.Rooms.FirstOrDefaultAsync(x=> x.Id == id);
            if(room != null)
            {
                 _dbContext.Rooms.Remove(room);
                await _dbContext.SaveChangesAsync();    
            }
            return Ok();    
        }
        [HttpPost]
        public async Task<IActionResult> AddRooms(Room room)
        {
            if (_dbContext.Rooms.Any(r => r.RoomCode == room.RoomCode))
            {
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 1,
                    Message = "Room da co",
                });
            }
            await _dbContext.Rooms.AddAsync(room);
            await _dbContext.SaveChangesAsync();
            return Ok(new ApiResponse
            {
                Success = true,
                Status = 0,
                Message = "Add room Successfully",
            });
        }
        [HttpPost("join")]
        public async Task<IActionResult> JoinRoom(Room room)
        {
            var existingRoom = await _dbContext.Rooms.FirstOrDefaultAsync(r => r.RoomCode == room.RoomCode);
            if (existingRoom == null)
            {
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 1,
                    Message = "Room not found",
                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Status = 0,
                Message = "Join room Successfully",
                Data = room.RoomCode
            });
        }
    }
}
