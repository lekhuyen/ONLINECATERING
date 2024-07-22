using APIRESPONSE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.DTOs;
using RESTAURANT.API.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComboController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ComboController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComboDTO>>> GetCombos()
        {
            var combos = await _context.Combos.Include(c => c.ComboDishes).ToListAsync();
            var comboDTOs = new List<ComboDTO>();

            foreach (var combo in combos)
            {
                comboDTOs.Add(new ComboDTO
                {
                    Id = combo.Id,
                    Name = combo.Name,
                    Price = combo.Price,
                    Status = combo.Status,
                    ImagePath = combo.ImagePath,
                    Type = combo.Type,
                    ComboDishes = combo.ComboDishes.Select(cd => new ComboDishDTO
                    {
                        ComboId = cd.ComboId,
                        DishId = cd.DishId
                    }).ToList()
                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Status = 0,
                Message = "Get combo Successfully",
                Data = comboDTOs
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ComboDTO>> GetComboById(int id)
        {
            var combo = await _context.Combos.Include(c => c.ComboDishes).FirstOrDefaultAsync(c => c.Id == id);

            if (combo == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Combo not found",
                });
            }

            var comboDTO = new ComboDTO
            {
                Id = combo.Id,
                Name = combo.Name,
                Price = combo.Price,
                Status = combo.Status,
                ImagePath = combo.ImagePath,
                Type = combo.Type,
                ComboDishes = combo.ComboDishes.Select(cd => new ComboDishDTO
                {
                    ComboId = cd.ComboId,
                    DishId = cd.DishId
                }).ToList()
            };

            return Ok(new ApiResponse
            {
                Success = true,
                Status = 0,
                Message = "Get combo Successfully",
                Data = comboDTO
            });
        }

        [HttpPost]
        public async Task<ActionResult<ComboDTO>> CreateCombo([FromForm] ComboDTO comboDTO)
        {
            if (comboDTO.ImageFile != null)
            {
                var filePath = Path.Combine("wwwroot", "images", comboDTO.ImageFile.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await comboDTO.ImageFile.CopyToAsync(stream);
                }

                comboDTO.ImagePath = filePath;
            }

            var combo = new Combo
            {
                Name = comboDTO.Name,
                Price = comboDTO.Price,
                Status = comboDTO.Status,
                ImagePath = comboDTO.ImagePath,
                Type = comboDTO.Type
            };

            _context.Combos.Add(combo);
            await _context.SaveChangesAsync();

            if (comboDTO.ComboDishes != null)
            {
                foreach (var comboDishDTO in comboDTO.ComboDishes)
                {
                    var comboDish = new ComboDish
                    {
                        ComboId = combo.Id,
                        DishId = comboDishDTO.DishId
                    };
                    _context.ComboDishes.Add(comboDish);
                }

                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetCombo), new { id = combo.Id }, comboDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCombo(int id, [FromForm] ComboDTO comboDTO)
        {
            if (id != comboDTO.Id)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Mismatched ID in about object and parameter",
                    Data = null
                });
            }

            var combo = await _context.Combos.Include(c => c.ComboDishes).FirstOrDefaultAsync(c => c.Id == id);
            if (combo == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Combo not found",
                });
            }

            if (comboDTO.ImageFile != null)
            {
                var filePath = Path.Combine("wwwroot", "images", comboDTO.ImageFile.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await comboDTO.ImageFile.CopyToAsync(stream);
                }

                comboDTO.ImagePath = filePath;
            }

            combo.Name = comboDTO.Name;
            combo.Price = comboDTO.Price;
            combo.Status = comboDTO.Status;
            combo.ImagePath = comboDTO.ImagePath;
            combo.Type = comboDTO.Type;

            _context.Entry(combo).State = EntityState.Modified;

            if (comboDTO.ComboDishes != null)
            {
                // Remove existing ComboDishes
                var existingComboDishes = _context.ComboDishes.Where(cd => cd.ComboId == id);
                _context.ComboDishes.RemoveRange(existingComboDishes);

                // Add new ComboDishes
                foreach (var comboDishDTO in comboDTO.ComboDishes)
                {
                    var comboDish = new ComboDish
                    {
                        ComboId = combo.Id,
                        DishId = comboDishDTO.DishId
                    };
                    _context.ComboDishes.Add(comboDish);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComboExists(id))
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Combo not found",
                    });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCombo(int id)
        {
            var combo = await _context.Combos.FindAsync(id);
            if (combo == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Combo not found",
                });
            }

            var comboDishes = _context.ComboDishes.Where(cd => cd.ComboId == id);
            _context.ComboDishes.RemoveRange(comboDishes);

            _context.Combos.Remove(combo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComboExists(int id)
        {
            return _context.Combos.Any(e => e.Id == id);
        }
    }
}
