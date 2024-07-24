using APIRESPONSE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.DTOs;
using RESTAURANT.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTAURANT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public UserController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            try
            {
                var users = await _dbContext.Users.ToListAsync();

                var userDTOs = users.Select(user => new UserDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    UserEmail = user.UserEmail,
                    Phone = user.Phone,
                }).ToList();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get users successfully",
                    Data = userDTOs
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Internal server error",
                    Data = ex.Message
                });
            }
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetUserById(int id)
        {
            try
            {
                var user = await _dbContext.Users.FindAsync(id);

                if (user == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "User not found",
                    });
                }

                var userDTO = new UserDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    UserEmail = user.UserEmail,
                    Phone = user.Phone,
                };

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get user successfully",
                    Data = userDTO
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

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateUser([FromBody] UserDTO userDTO)
        {
            try
            {
                // Map DTO to entity
                var newUser = new User
                {
                    UserName = userDTO.UserName,
                    UserEmail = userDTO.UserEmail,
                    Phone = userDTO.Phone
                };

                // Add to DbContext
                await _dbContext.Users.AddAsync(newUser);
                await _dbContext.SaveChangesAsync();

                // Prepare response DTO
                var createdUserDTO = new UserDTO
                {
                    Id = newUser.Id,
                    UserName = newUser.UserName,
                    UserEmail = newUser.UserEmail,
                    Phone = newUser.Phone,
                };

                return Created("success", new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Add user successfully",
                    Data = createdUserDTO
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

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateUser(int id, [FromBody] UserDTO userDTO)
        {
            try
            {
                var existingUser = await _dbContext.Users.FindAsync(id);

                if (existingUser == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "User not found",
                    });
                }

                if (id != userDTO.Id)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Mismatched ID in object and parameter",
                        Data = null
                    });
                }

                // Update scalar properties
                existingUser.UserName = userDTO.UserName;
                existingUser.UserEmail = userDTO.UserEmail;
                existingUser.Phone = userDTO.Phone;

                // Update entity in DbContext
                _dbContext.Users.Update(existingUser);
                await _dbContext.SaveChangesAsync();

                // Prepare response DTO
                var updatedUserDTO = new UserDTO
                {
                    Id = existingUser.Id,
                    UserName = existingUser.UserName,
                    UserEmail = existingUser.UserEmail,
                    Phone = existingUser.Phone,
                };

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "User updated successfully",
                    Data = updatedUserDTO
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Error from service: " + ex.Message,
                    Data = null
                });
            }
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteUser(int id)
        {
            try
            {
                var userToDelete = await _dbContext.Users.FindAsync(id);

                if (userToDelete == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "User not found",
                    });
                }

                // Remove from DbContext and save changes
                _dbContext.Users.Remove(userToDelete);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Delete user successfully",
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

        // Helper method to check if a user exists
        private bool UserExists(int id)
        {
            return _dbContext.Users.Any(e => e.Id == id);
        }
    }
}
