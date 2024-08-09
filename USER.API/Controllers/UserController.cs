
using APIRESPONSE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using REDISCLIENT;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using USER.API.DTOs;
using USER.API.Helpers;
using USER.API.Models;
using USER.API.Repositories;
using USER.API.Service;

namespace USER.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IRepositories _repository;
		private IConfiguration _configuration;
		private readonly IAuthUser _authUser;
		private readonly RedisClient _redisClient;

		private readonly DatabaseContext _databaseContext;
		private readonly EmailServices _emailServices;

		public UserController(IAuthUser authUser,
								IRepositories repositories,
								IConfiguration configuration,
								DatabaseContext databaseContext,
								EmailServices emailServices,
								RedisClient redisClient)
		{
			_repository = repositories;
			_configuration = configuration;
			_authUser = authUser;
			_redisClient = redisClient;
			_databaseContext = databaseContext;
			_emailServices = emailServices;
		}

		[HttpGet]
		//[Authorize(Roles = "Admin")]
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
					Message = "Error getting user list",
					Data = null
				});
			}

		}
		[HttpPost]
		public async Task<IActionResult> AddUser(User user)
		{
			try
			{
				if (ModelState.IsValid)
				{
					// Generate confirmation token
					//var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
					var token = RandomString(6);
					user.ConfirmationToken = token;
					user.ConfirmationTokenExpiry = DateTime.UtcNow.AddMinutes(2); // Set token expiry

					var userRes = await _repository.AddUserAsync(user);
					//await _databaseContext.Users.InsertOneAsync(user);
					if (userRes == 1)
					{
						return Ok(new ApiResponse
						{
							Success = false,
							Status = 1,
							Message = "Email is already existed"
						});
					}
					else if (userRes == 2)
					{
						return Ok(new ApiResponse
						{
							Success = false,
							Status = 1,
							Message = "Phone is already existed"
						});
					}

					//reids
					var userDTO = new UserDTO
					{
						UserName = user.UserName,
						UserEmail = user.UserEmail,
						Phone = user.Phone,
					};
					var userJson = JsonConvert.SerializeObject(userDTO);
					_redisClient.Publish("user_created", userJson);

					// Send confirmation email
					//var confirmationLink = Url.Action("ConfirmEmail", "User", new { token }, Request.Scheme);
					var confirmationLink = $"http://localhost:3000/login/{token}";
					var confirmationMessage = $"Dear {user.UserName},<br><br>Thank you for registering. Please confirm your email by clicking the link below:<br><br><a href='{confirmationLink}'>Confirm Email</a>";
					var emailRequest = new EmailRequest
					{
						ToMail = user.UserEmail,
						Subject = "Confirm your email",
						HtmlContent = confirmationMessage
					};
					await _emailServices.SendEmailAsync(emailRequest);

					return Created("success", new ApiResponse
					{
						Success = true,
						Status = 0,
						Message = "Create user successfully, please confirm your email to continue login"
					});
				}
				return Ok(new ApiResponse
				{
					Success = false,
					Status = 1,
					Message = "Create user failed"
				});
			}
			catch (Exception ex)
			{
				return Ok(new ApiResponse
				{
					Success = false,
					Status = 1,
					Message = "Error creating user"
				});
			}
		}

		[HttpGet("ConfirmEmail")]
		public async Task<IActionResult> ConfirmEmail(string token)
		{
			if (string.IsNullOrEmpty(token))
			{
				return BadRequest(new ApiResponse
				{
					Success = false,
					Status = 1,
					Message = "Invalid token"
				});
			}

			var user = await _repository.GetByIdRefeshToken(token);
			if (user == null || user.ConfirmationTokenExpiry < DateTime.UtcNow)
			{
				return BadRequest(new ApiResponse
				{
					Success = false,
					Status = 1,
					Message = "Invalid or expired token"
				});
			}

			user.EmailConfirmed = true;
			user.ConfirmationToken = null;
			user.ConfirmationTokenExpiry = null;

			await _repository.UpdateUserAsync(user);

			return Ok(new ApiResponse
			{
				Success = true,
				Status = 0,
				Message = "Email confirmed successfully"
			});
		}


		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var user = await _repository.GetByIdAsync(id);

			if (user == null)
			{

				return NotFound(new ApiResponse
				{
					Success = false,
					Status = 1,
					Message = "Get user failed"
				});
			}

			return Ok(new ApiResponse
			{
				Success = true,
				Status = 0,
				Message = "Get user Successfully",
				Data = user
			});
		}

		[HttpGet("role/{roleAdmin}")]
		public async Task<IActionResult> GetByEmail(string roleAdmin)
		{
			var user = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Role == roleAdmin);


			if (user == null)
			{

				return NotFound(new ApiResponse
				{
					Success = false,
					Status = 1,
					Message = "Get user failed"
				});
			}

			return Ok(new ApiResponse
			{
				Success = true,
				Status = 0,
				Message = "Get user Successfully",
				Data = user
			});
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateUser(int id, User user)
		{
			if (id != user.Id)
			{
				return BadRequest("Id is mismatched");
			}

			try
			{
				var userExisted = await _repository.GetOneByIdAsync(id);
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
								Message = "Email is already existed"
							});
						}
						else if (userRes == 2)
						{
							return BadRequest(new ApiResponse
							{
								Success = false,
								Status = 1,
								Message = "Phone is already existed"
							});
						}

						//reids
						var userDTO = new UserDTO
						{
							Id = id,
							UserName = user.UserName,
							UserEmail = user.UserEmail,
							Phone = user.Phone,
						};
						var userJson = JsonConvert.SerializeObject(userDTO);
						_redisClient.Publish("user_update", userJson);

						return Ok(new ApiResponse
						{
							Success = true,
							Status = 0,
							Message = "Update user Successfully",
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
			catch (Exception ex)
			{
				return BadRequest(new ApiResponse
				{
					Success = false,
					Status = 1,
					Message = "Error updating user",
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
				if (userExisted != null)
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
					Message = "User is not existed",
				});
			}
			catch (Exception ex)
			{
				return BadRequest(new ApiResponse
				{
					Success = false,
					Status = 1,
					Message = "Error deleting user",
					Data = null
				});
			}
		}

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login login)
        {
            var userLogin = await _authUser.Login(login);
            if (userLogin == null)
            {
                return Ok(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Wrong Email or Password"
                });
            }

            // Check if the account is banned
            if (userLogin.Status)  // Assuming Status = false means active and Status = true means banned
            {
                return Ok(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Your account has been banned"
                });
            }

            // Token and refresh token generation
            var token = GenerateToken(userLogin);

            var refreshToken = Guid.NewGuid().ToString();
            var refreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(2);

            userLogin.RefeshToken = refreshToken;
            userLogin.RefreshTokenExpiryTime = refreshTokenExpiryTime;
            await _repository.UpdateUserAsync(userLogin);

            var userDTO = new UserDTO
            {
                Id = userLogin.Id,
                UserEmail = userLogin.UserEmail,
                UserName = userLogin.UserName,
                Phone = userLogin.Phone,
                Role = userLogin.Role,
                AccessToken = token,
                RefeshToken = refreshToken
            };

            return Ok(new ApiResponse
            {
                Success = true,
                Status = 0,
                Message = "Login successfully",
                Data = userDTO
            });
        }


        [HttpPost("login-token")]
		public async Task<IActionResult> LoginToken(Login login)
		{
			var userLogin = await _authUser.Login(login);
			if (userLogin == null)
			{
				return Ok(new ApiResponse
				{
					Success = false,
					Status = 1,
					Message = "Wrong Email or Password"
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
				Id = userLogin.Id,
				UserEmail = userLogin.UserEmail,
				UserName = userLogin.UserName,
				Phone = userLogin.Phone,
				Role = userLogin.Role,
				AccessToken = token,
				RefeshToken = refeshToken
			};

			return Ok(new ApiResponse
			{
				Success = true,
				Status = 0,
				Message = "Login successfully",
				//AccessToken = token,
				//RefreshToken = refeshToken,
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
			if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
			{
				return BadRequest(new ApiResponse
				{
					Success = false,
					Status = 1,
					Message = "Login timeout, please login again",
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
		private string RandomString(int length)
		{
			var random = new Random();
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, length)
				.Select(s => s[random.Next(s.Length)]).ToArray());
		}

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(Login useEmail)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var user = await _databaseContext.Users.FirstOrDefaultAsync(u => u.UserEmail == useEmail.UserEmail);

					if (user != null)
					{
						var otp = RandomString(6);

						var emailRequest = new EmailRequest
						{
							HtmlContent = $"Your OTP is {otp}",
							ToMail = useEmail.UserEmail!,
							Subject = "ForgotPassword"
						};

						await _emailServices.SendEmailAsync(emailRequest);

						user.Otp = otp;
						user.OtpExpired = DateTime.UtcNow.AddMinutes(1);
						_databaseContext.Users.Update(user);
						await _databaseContext.SaveChangesAsync();

						return Ok(new ApiResponse
						{
							Success = true,
							Status = 0,
							Message = "OTP has been sent to your mail",
							Data = user.UserEmail
						});
					}
					else
					{
						return Ok(new ApiResponse
						{
							Success = false,
							Status = 1,
							Message = "Email is incorrect or does not exist "
						});
					}
				}
				return Ok(new ApiResponse
				{
					Success = false,
					Status = 1,
					Message = "Email is empty"
				});

            }
            catch(Exception ex)
            {
                return Ok(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Server something wrong "
                });
            }
        }

		[HttpPost("otp")]
		public async Task<IActionResult> Otp(Login login)
		{
			try
			{
				var user = await _databaseContext.Users.FirstOrDefaultAsync(u => u.UserEmail == login.UserEmail);

				if (user != null)
				{
					if (user.Otp != login.Otp)
					{
						return Ok(new ApiResponse
						{
							Success = false,
							Status = 1,
							Message = "OTP is incorrect"
						});
					}
					if (user.OtpExpired <= DateTime.UtcNow)
					{
						return Ok(new ApiResponse
						{
							Success = false,
							Status = 1,
							Message = "OTP has expired"
						});
					}
					user.Otp = "";
					user.OtpExpired = null;


					_databaseContext.Users.Update(user);
					await _databaseContext.SaveChangesAsync();

					return Ok(new ApiResponse
					{
						Success = true,
						Status = 0,
						Data = user.UserEmail
					});
				}
				else
				{
					return Ok(new ApiResponse
					{
						Success = false,
						Status = 1,
						Message = "OTP is incorrect or expired"
					});
				}
			}
			catch (Exception ex)
			{
				return Ok(new ApiResponse
				{
					Success = false,
					Status = 1,
					Message = "Error from server"
				});
			}
		}

		[HttpPost("update-password")]
		public async Task<IActionResult> UpdatePassword(Login login)
		{
			try
			{
				var user = await _databaseContext.Users.FirstOrDefaultAsync(u => u.UserEmail == login.UserEmail);

				if (user != null)
				{
					//if (!PasswordBcrypt.VerifyPassword(login.Password, user.Password))
					//{
					//	return Ok(new ApiResponse
					//	{
					//		Success = false,
					//		Status = 1,
					//		Message = "Old password must be matched with login password"
					//	});
					//}

					user.Password = PasswordBcrypt.HashPassword(login.Password);

					_databaseContext.Users.Update(user);
					await _databaseContext.SaveChangesAsync();

					return Ok(new ApiResponse
					{
						Success = true,
						Status = 0,
						Message = "Password has been changed, please login again"
					});
				}
				else
				{
					return Ok(new ApiResponse
					{
						Success = false,
						Status = 1,
						Message = "User not found"
					});
				}
			}
			catch (Exception ex)
			{
				return Ok(new ApiResponse
				{
					Success = false,
					Status = 1,
					Message = "Internal server error"
				});
			}
		}

		[HttpPost("update-password-otp")]
		public async Task<IActionResult> UpdatePasswordUser(Login login)
		{
			try
			{
                // Validate the length of the old and new passwords
                if (login.OldPassword.Length < 6 || login.Password.Length < 6)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Passwords must be at least 6 characters long"
                    });
                }

                var user = await _databaseContext.Users.FirstOrDefaultAsync(u => u.UserEmail == login.UserEmail);

				if (user != null)
				{
                    bool veriPass = PasswordBcrypt.VerifyPassword(login.OldPassword, user.Password);
                    if (veriPass)
                    {
						if (user.Otp == login.Otp)
						{
                            
                            user.Password = PasswordBcrypt.HashPassword(login.Password);

                            _databaseContext.Users.Update(user);
                            await _databaseContext.SaveChangesAsync();

                            return Ok(new ApiResponse
                            {
                                Success = true,
                                Status = 0,
                                Message = "Succ"
                            });
                        }
						return Ok(new ApiResponse
						{
							Success = false,
							Status = 1,
							Message = "Wrong OTP"
						});


					}
                    else
                    {
						return Ok(new ApiResponse
						{
							Success = false,
							Status = 1,
							Message = "Wrong Password"
						});
					}

                }
				else
				{
					return Ok(new ApiResponse
					{
						Success = false,
						Status = 1,
						Message = "User not found"
					});
				}
			}
			catch (Exception ex)
			{
				return Ok(new ApiResponse
				{
					Success = false,
					Status = 1,
					Message = "Internal server error"
				});
			}
		}

        [HttpPut("admin-edit/{id}")]
        public async Task<IActionResult> AdminEditUserStatus(int id, [FromQuery] int userId, [FromQuery] bool newStatus)
        {
            if (id != userId)
            {
                return BadRequest("User ID mismatch.");
            }

            try
            {
                var user = await _databaseContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null)
                {
                    bool wasPreviouslyBanned = user.Status;
                    bool isCurrentlyBanned = newStatus;

                    if (wasPreviouslyBanned != isCurrentlyBanned)
                    {
                        if (isCurrentlyBanned)
                        {
                            // Send email when user is banned
                            await _emailServices.SendBanNotificationEmail(user.UserEmail, user.UserName);
                        }
                        else
                        {
                            // Send email when user is unbanned
                            await _emailServices.SendUnbanNotificationEmail(user.UserEmail, user.UserName);
                        }
                    }

                    user.Status = newStatus;
                    await _databaseContext.SaveChangesAsync();
                    return Ok(user); // Return the updated user object
                }

                return NotFound("User not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Error from server: " + ex.Message
                });
            }
        }

    }
}
