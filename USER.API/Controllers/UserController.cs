
using APIRESPONSE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using USER.API.DTOs;
using USER.API.Helpers;
using USER.API.Models;
using USER.API.Repositories;

namespace USER.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepositories _repository;
        private IConfiguration _configuration;
        private readonly IAuthUser _authUser;



        public UserController(IAuthUser authUser,IRepositories repositories, IConfiguration configuration)
        {
            _repository = repositories;
            _configuration = configuration;
            _authUser = authUser;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                var users = await _repository.GetAllUserAsync();
                var userDto = users.Select(u => new UserDTO
                {
                    Id = u.Id,
                    UserEmail = u.UserEmail,
                    UserName = u.UserName,
                    Phone = u.Phone,
                    Role = u.Role,
                    Password = u.Password,
                    Grade = u.Grade != null ? new GradeDTO
                    {
                        Point = u.Grade.Point
                    } : null,
                    FavoriteList = u.FavoriteLists != null ? u.FavoriteLists.Select(f => new FavoriteListDTO
                    {
                        RestaurantName = f.RestaurantName,
                        Image = f.Image,
                        Address = f.Address,
                        Rating = f.Rating,
                    }).ToList() : null,

                }).ToList();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get Users Successfully",
                    Data = userDto
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
            //var users = await _databaseContext.Users.Find(u => true).ToListAsync();
            
            
        }
        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userRes = await _repository.AddUserAsync(user);
                    //await _databaseContext.Users.InsertOneAsync(user);
                    if (userRes == 1)
                    {
                        return BadRequest(new ApiResponse
                        {
                            Success = false,
                            Status = 1,
                            Message = "Email da ton tai"
                        });
                    }
                    else if (userRes == 2)
                    {
                        return BadRequest(new ApiResponse
                        {
                            Success = false,
                            Status = 1,
                            Message = "Phone da ton tai"
                        });
                    }
                    return Created("success", new ApiResponse
                    {
                        Success = true,
                        Status = 0,
                        Message = "User added successfully"
                    });
                }
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Create Users failed"
                });
            }catch(Exception ex) {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Create Users failed"
                });
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            //var user = await _databaseContext.Users.Find(u => u.Id == id).FirstOrDefaultAsync();
            if(user == null)
            {
                
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Get Users failed"
                });
            }
            var userDTO = new UserDTO
            {
                UserEmail = user.UserEmail,
                UserName = user.UserName,
                Phone = user.Phone,
                Grade = new GradeDTO
                {
                    Point = user.Grade != null ? user.Grade.Point : null
                },
                FavoriteList = user.FavoriteLists != null ? user.FavoriteLists.Select(f => new FavoriteListDTO
                {
                    RestaurantName = f.RestaurantName,
                    Image = f.Image,
                    Address = f.Address,
                    Rating = f.Rating,
                }).ToList() : null,

            };
            return Ok(new ApiResponse
            {
                Success = true,
                Status = 0,
                Message = "Get Users Successfully",
                Data = userDTO
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest("Id khong trung nhau");
            }

            try
            {
                var userExisted = await _repository.GetByIdAsync(id);
                if (userExisted != null)
                {
                    if (ModelState.IsValid)
                    {
                        userExisted.UserEmail = user.UserEmail;
                        userExisted.UserName = user.UserName;
                        userExisted.Phone = user.Phone;
                        userExisted.Role = user.Role;
                        userExisted.Status = user.Status;
                        userExisted.Password = PasswordBcrypt.HashPassword(user.Password);

                        var userRes = await _repository.UpdateUserAsync(userExisted);

                        if (userRes == 1)
                        {
                            return BadRequest(new ApiResponse
                            {
                                Success = false,
                                Status = 1,
                                Message = "Email da ton tai"
                            });
                        }
                        else if (userRes == 2)
                        {
                            return BadRequest(new ApiResponse
                            {
                                Success = false,
                                Status = 1,
                                Message = "Phone da ton tai"
                            });
                        }
                        return Ok(new ApiResponse
                        {
                            Success = true,
                            Status = 0,
                            Message = "Update Users Successfully",
                            Data = user
                        });

                    }
                    
                }

                return NotFound(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "User not found",
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

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var userExisted = await _repository.GetByIdAsync(id);
                if(userExisted != null)
                {
                    await _repository.DeleteUserAsync(id);
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Status = 0,
                        Message = "Delete user successfully",
                    });
                }
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "User is not userExist",
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

        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var userLogin = await _authUser.Login(email, password);
            if (userLogin == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Email or Password is wrong"
                });
            }


            //token ------------  refeshToken
            var token = GenerateToken(userLogin);

            var refeshToken = Guid.NewGuid().ToString();
            var refreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(2);


            userLogin.RefeshToken = refeshToken;
            userLogin.RefreshTokenExpiryTime = refreshTokenExpiryTime;
            await _repository.UpdateUserAsync(userLogin);

            var userDTO = new UserDTO
            {
                UserEmail = userLogin.UserEmail,
                UserName = userLogin.UserName,
                Phone = userLogin.Phone,
                Role = userLogin.Role,
            };

            return Ok(new ApiResponse
            {
                Success = true,
                Status = 0,
                Message = "Logged in successfully",
                AccessToken = token,
                RefreshToken = refeshToken,
                Data = userDTO
            });

        }

        private string GenerateToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.Email, user.UserEmail),
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }


        
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(string tokenRefresh)
        {
            var user = await _repository.GetByIdRefeshToken(tokenRefresh);
            if(user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Login timeout, please log in again",
                });
            }
            var tokenString = GenerateToken(user);

            var refreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(2);
            var refeshToken = Guid.NewGuid().ToString();

            user.RefeshToken = refeshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiryTime;
            await _repository.UpdateUserAsync(user);

            return Ok(new ApiResponse
            {
                Data = user,
                AccessToken = tokenString,
                RefreshToken = refeshToken
            });

        }
        
    }
}
