
using APIRESPONSE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        private readonly DatabaseContext _dbContext;



        public UserController(IAuthUser authUser,IRepositories repositories, IConfiguration configuration, DatabaseContext dbContext)
        {
            _repository = repositories;
            _configuration = configuration;
            _authUser = authUser;
            _dbContext = dbContext;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                var users = await _repository.GetAllUserAsync();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get Users Successfully",
                    Data = users
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
            if(ModelState.IsValid)
            {
                await _repository.AddUserAsync(user);
                //await _databaseContext.Users.InsertOneAsync(user);
                return Created("success",new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Create Users Successfully"
                });
            }
            return BadRequest(new ApiResponse
            {
                Success = false,
                Status = 1,
                Message = "Create Users failed"
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
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
            return Ok(new ApiResponse
            {
                Success = true,
                Status = 0,
                Message = "Get Users Successfully",
                Data = user
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, User user)
        {
            try
            {
                var userExisted = await _repository.GetByIdAsync(id);
                if( userExisted is null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "User notfound"
                    });
                }
                await _repository.UpdateUserAsync(id, userExisted);
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Update Users Successfully",
                    Data = user
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
        public async Task<IActionResult> DeleteUser(string id)
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

            var r = Builders<User>.Update
                .Set(u => u.RefeshToken, refeshToken);

            var e = Builders<User>.Update
                .Set(u => u.RefreshTokenExpiryTime, refreshTokenExpiryTime);

             _dbContext.Users.UpdateOne(u => u.Id == userLogin.Id, r);
             _dbContext.Users.UpdateOne(u => u.Id == userLogin.Id, e);

            return Ok(new ApiResponse
            {
                Success = true,
                Status = 0,
                Message = "Logged in successfully",
                AccessToken = token,
                RefreshToken = refeshToken
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
                return Unauthorized();
            }
            var tokenString = GenerateToken(user);

            var refreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(2);
            var refeshToken = Guid.NewGuid().ToString();

            var r = Builders<User>.Update
                .Set(u => u.RefeshToken, refeshToken);

            var e = Builders<User>.Update
                .Set(u => u.RefreshTokenExpiryTime, refreshTokenExpiryTime);

            _dbContext.Users.UpdateOne(u => u.Id == user.Id, r);
            _dbContext.Users.UpdateOne(u => u.Id == user.Id, e);

            return Ok(new ApiResponse
            {
                Data = user,
                AccessToken = tokenString,
                RefreshToken = refeshToken
            });

        }
        
    }
}
