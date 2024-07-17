using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.Models;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        public BookingController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetBooking()
        {
            var booking = await _dbContext.Booking.ToListAsync();
            return Ok(booking);
        }
        [HttpGet("user")]
        public async Task<IActionResult> GetUser()
        {
            var users = await _dbContext.User.ToListAsync();
            return Ok(users);
        }
    }
}
