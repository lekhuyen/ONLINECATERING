using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USER.API.Models;

namespace USER.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBookingController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        public UserBookingController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        [HttpGet("booking")]
        public async Task<IActionResult> GetBooking()
        {
            var booking = await _databaseContext.UserBooking.ToListAsync();
            return Ok(booking);
        }
    }
}
