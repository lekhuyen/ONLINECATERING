using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using REDISCLIENT;
using System.Text;
using System.Text.Json;
using USER.API.DTOs;
using USER.API.Helpers;
using USER.API.Models;

namespace USER.API.Repositories
{
    public class UserRepositories : IRepositories
    {
        private readonly DatabaseContext _dbContext;
        public UserRepositories(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddUserAsync(User user)
        {
            
            var userExistEmail = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserEmail == user.UserEmail);

            if (userExistEmail != null)
            {
                return 1;
            }

            var userExistPhone = await _dbContext.Users.FirstOrDefaultAsync(u => u.Phone == user.Phone);
                        
            if (userExistPhone != null)
            {
                 return 2;
            }


            
            user.Password = PasswordBcrypt.HashPassword(user.Password);
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return 0;
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if(user != null)
            {
                 _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> GetAllUserAsync()
        {

            var users = await _dbContext.Users
                .Include(u => u.Grade)
                .Include(u => u.FavoriteLists)
                .Include(u => u.UserBookings)
                .ToListAsync();
            return users;
        }

        public async Task<UserDTO> GetByIdAsync(int id)
        {
            var user = await _dbContext.Users
                .Include(u=> u.Grade)
                .Include(u => u.FavoriteLists)
                .Include(u => u.UserBookings)
                .FirstOrDefaultAsync(u => u.Id == id);

            
            var userDTO = new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                Phone = user.Phone,
                Role = user.Role,
                Status = user.Status,
                Booking = user.UserBookings.Select(u => new BookingDTO
                {
                    Id = u.Id,
                    UserId = u.UserId,
                    RestaurantId = u.RestaurantId,
                    MenuId = u.MenuId,
                    Member = u.Member,
                    DayArrive = u.DayArrive,
                    Hour = u.Hour,
                    Status = u.Status,
                    Pont = u.Pont,
                    Total = u.Total,
                    Description = u.Description,
                }).ToList(),
                Grade = new GradeDTO
                {
                    Point = user.Grade.Point,
                },
                FavoriteList = user.FavoriteLists.Select(u => new FavoriteListDTO
                {
                    RestaurantName = u.RestaurantName,
                    UserId = user.Id,
                    Image = u.Image,
                    Address = u.Address,
                    Rating = u.Rating,
                }).ToList(),

            };
            return userDTO;
        }

        public async Task<User> GetByIdRefeshToken(string refeshToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.RefeshToken == refeshToken);
            return user;
        }
        public async Task<User> GetOneByIdAsync(int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }


        public async Task<int> UpdateUserAsync(User user)
        {
            var userExistEmail = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserEmail == user.UserEmail);

            if (userExistEmail != null)
            {
                return 1;
            }

            var userExistPhone = await _dbContext.Users.FirstOrDefaultAsync(u => u.Phone == user.Phone);

            if (userExistPhone != null)
            {
                return 2;
            }

            //user.Password = PasswordBcrypt.HashPassword(user.Password);

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return 0;
        }
    }
}
