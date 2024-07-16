using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using USER.API.Models;

namespace USER.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        public RestaurantController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        [HttpGet("restaurant")]
        public async Task<IActionResult> GetRestaurant()
        {
            var res = await _databaseContext.Restaurants.ToListAsync();
            return Ok(res);
        } 
        [HttpGet("menu")]
        public async Task<IActionResult> GetMenu()
        {
            var res = await _databaseContext.Menus.ToListAsync();
            return Ok(res);
        }
    }
}
