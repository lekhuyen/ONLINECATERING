using APIRESPONSE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.DTOs;
using RESTAURANT.API.Models;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComboDishController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public ComboDishController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var comboDishes = await _dbContext.ComboDishes
                    .Include(cd => cd.Combo)
                    .Include(cd => cd.Dish)
                    .ToListAsync();

                var list = comboDishes.Select(cd => new ComboDishDTO
                {
                    DishName = cd.Dish.Name,
                    ComboName = cd.Combo.Name,
                }).ToList();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get ComboDisks successfully",
                    Data = comboDishes
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
