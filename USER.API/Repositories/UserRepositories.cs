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
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<UserDTO>> GetAllUserAsync()
        {

            var users = await _dbContext.Users
                .Include(u => u.Grade)
                //.Include(u => u.Restaurants)
                .Include(u => u.FavoriteLists)
                .Include(u => u.UserBookings)
                .ThenInclude(u => u.Restaurant)
                .ToListAsync();

            Booking userBooking = null;
            List<Menu> menus = null;
            decimal total = 0;
            foreach (var user in users)
            {
                userBooking = user.UserBookings.FirstOrDefault();
                if(userBooking != null && userBooking.MenuId != null)
                {
                    menus = await _dbContext.Menus.Where(m => userBooking.MenuId.Contains(m.Id.ToString())).ToListAsync();
                }
            }
            if(menus != null)
            {
                foreach (var menu in menus)
                {
                    total = (menu.Price * menu.Quatity) + total;
                }
            }

            var userDto = users.Select(u => new UserDTO
            {
                Id = u.Id,
                UserEmail = u.UserEmail,
                UserName = u.UserName,
                Phone = u.Phone,
                Role = u.Role,
                Password = u.Password,
                Otp = u.Otp,
                OtpExpired = u.OtpExpired,
                //Restaurants = u.Restaurants.Select(u => new RestaurantDTO
                //{
                //    Id = u.Id,
                //    RestaurantName = u.RestaurantName,
                //    City = u.City,
                //    Address = u.Address,
                //}).ToList(),
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
                Booking = u.UserBookings != null ? u.UserBookings.Select(u => new BookingDTO
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
                    Total = total,
                    Description = u.Description,
                    Menu = menus.Select(m => new MenuDTO
                    {
                        Id = m.Id,
                        MenuImage = m.MenuImage,
                        MenuName = m.MenuName,
                        Quatity = m.Quatity,
                        Price = m.Price,
                    }).ToList(),
                    Restaurant = u.Restaurant.Select(r => new RestaurantDTO
                    {
                        Id = r.Id,
                        RestaurantName = r.RestaurantName,
                        Address = r.Address,
                        City = r.City,

                    }).ToList(),
                    
                }).ToList() : null,
            }).ToList();
            return userDto;
        }

        public async Task<UserDTO> GetByIdAsync(int id)
        {
            var user = await _dbContext.Users
                .Include(u => u.Grade)
                //.Include(u => u.Restaurants)
                .Include(u => u.FavoriteLists)
                .Include(u => u.UserBookings)
                .ThenInclude(u => u.Restaurant)
                .FirstOrDefaultAsync(u => u.Id == id);

            
            var userBooking = user.UserBookings.FirstOrDefault();
            List<Menu> m = null;
            decimal total = 0;
            if (userBooking != null && userBooking.MenuId != null)
            {
                m = await _dbContext.Menus.Where(m => userBooking.MenuId.Contains(m.Id.ToString())).ToListAsync();
            }
            if(m != null)
            {
                foreach (var menu in m)
                {
                    total = (menu.Price * menu.Quatity) + total;
                }
            }
            


            var userDTO = new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                Phone = user.Phone,
                Role = user.Role,
                Status = user.Status,
                Otp = user.Otp,
                OtpExpired = user.OtpExpired,
                
                Booking = user?.UserBookings?.Select(u => new BookingDTO
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
                    Total = total,
                    Description = u.Description,
                    Menu = m.Select(menu => new MenuDTO
                    {
                        Id = menu.Id,
                        MenuImage = menu.MenuImage,
                        MenuName = menu.MenuName,
                        Quatity = menu.Quatity,
                        Price = menu.Price
                    }).ToList(),
                    Restaurant = u.Restaurant.Select(r => new RestaurantDTO
                    {
                        Id = r.Id,
                        RestaurantName = r.RestaurantName,
                        Address = r.Address,
                        City = r.City,

                    }).ToList(),
                }).ToList() ?? new List<BookingDTO>(),
                Grade = user.Grade != null ? new GradeDTO
                {
                    Point = user.Grade.Point,
                } : null,
                FavoriteList = user.FavoriteLists?.Select(u => new FavoriteListDTO
                {
                    RestaurantName = u.RestaurantName,
                    UserId = user.Id,
                    Image = u.Image,
                    Address = u.Address,
                    Rating = u.Rating,
                }).ToList() ?? new List<FavoriteListDTO>(),

            };
            if (userDTO == null)
            {
                return null;
            }
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
