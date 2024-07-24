using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using REDISCLIENT;
using RESTAURANT.API.DTOs;
using RESTAURANT.API.Models;

namespace RESTAURANT.API.Repositories
{
    public class RestaurantRepositories : IRestaurant
    {
        private readonly RedisClient _redisClient;
        private readonly DatabaseContext _dbContext;
        public RestaurantRepositories(DatabaseContext dbContext, RedisClient redisClient)
        {
            _dbContext = dbContext;
            _redisClient = redisClient;
        }
        public async Task<PostRestaurantDTO> AddRestaurantAsync(PostRestaurantDTO restaurant)
        {
            var res = new Restaurant
            {
                RestaurantName = restaurant.RestaurantName,
                City = restaurant.City,
                Address = restaurant.Address,
                Open = restaurant.Open,
                Close = restaurant.Close,
                CategoryId = (int)restaurant.CategoryId,
                UserId = (int)restaurant.UserId,
            };

            var restaurantJson = JsonConvert.SerializeObject(res);
            _redisClient.Publish("created_restaurant", restaurantJson);

            await _dbContext.Restaurants.AddAsync(res);
            await _dbContext.SaveChangesAsync();
            return restaurant;
        }

        public async Task DeleteRestaurantAsync(int id)
        {
            var restaurant = await _dbContext.Restaurants.FirstOrDefaultAsync(x => x.Id == id);
            if (restaurant != null)
            {
                 _dbContext.Restaurants.Remove(restaurant);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<Restaurant> GetRestaurantByIdAsync(int id)
        {
            var restaurant = await _dbContext.Restaurants
                
                .FirstOrDefaultAsync(x => x.Id == id);

            
            return restaurant;
        }

        public async Task<GetRestaurantDTO> GetOneRestaurantByIdAsync(int id)
        {
            var restaurant = await _dbContext.Restaurants
                .Include(r => r.Category)
                .Include(r => r.Rating)
                .Include(r => r.Comment)
                .ThenInclude(r => r.CommentChildren)
                .Include(r => r.RestaurantImages)
                .Include(r => r.Bookings)
                .ThenInclude(r => r.User)
                .Include(r => r.Menus)
                .FirstOrDefaultAsync(x => x.Id == id);

            var bookings = restaurant?.Bookings?.ToList();

            List<Menu> menus = new List<Menu>();
            decimal total = 0;
            foreach (var booking in bookings)
            {
                if (booking.MenuId != null)
                {
                    var menuIds = booking.MenuId.Select(id => int.Parse(id)).ToList();
                    var bookingMenus = await _dbContext.Menus.Where(m => menuIds.Contains(m.Id)).ToListAsync();
                    menus.AddRange(bookingMenus);

                    foreach (var menu in bookingMenus)
                    {
                        total += (menu.Price * (menu.Quantity));
                    }
                }
            }

            var restaurantDTO = new GetRestaurantDTO
            {
                Id = restaurant.Id,
                RestaurantName = restaurant.RestaurantName,
                City = restaurant.City,
                UserId = (int)restaurant.UserId,
                Address = restaurant.Address,
                Open = restaurant.Open,
                Close = restaurant.Close,
                DisCount = restaurant.DisCount,
                Status = restaurant.Status,
                Warning = (int)restaurant.Warning,
                TotalRating = restaurant.TotalRating,
                Booking = restaurant.Bookings != null ? restaurant.Bookings.Select(u => new BookingDTO
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
                    User = new UserDTO
                    {
                        Id = u.User.Id,
                        UserEmail = u.User.UserEmail,
                        UserName = u.User.UserName,
                        Phone = u.User.Phone
                    },
                    Menu = menus?.Select(m => new GetMenuDTO
                    {
                        Id = m.Id,
                        MenuImage = m.MenuImage,
                        MenuName = m.MenuName,
                        Price = m.Price,
                        Quantity = m.Quantity 
                    }).ToList(),

                }).ToList() : null,
                Menus = restaurant?.Menus?.Select(m => new GetMenuDTO
                {
                    Id = m.Id,
                    MenuName = m.MenuName,
                    Price = m.Price,
                    Ingredient = m.Ingredient,
                    MenuImage = m.MenuImage,
                }).ToList(),
                RestaurantImages = restaurant?.RestaurantImages?.Select(m => new RestaurantImagesDTO
                {
                    Id = m.Id,
                    ImagesUrl = m.ImagesUrl,
                }).ToList(),
                Comment = restaurant?.Comment?.Select(c => new CommentDTO
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    Content = c.Content,
                    CommentChildren = c.CommentChildren?.Select(x => new CommentChildDTO
                    {
                        Id = x.Id,
                        CommentId = x.CommentId,
                        UserId = x.Id,
                        Content = x.Content,
                    }).ToList(),
                }).ToList(),
                Category = new CategoryDTO
                {
                    Title = restaurant.Category.Title
                },
                Rating = restaurant.Rating.Select(r => new RatingDTO
                {
                    Id = r.Id,
                    Point = r.Point,
                    UserId = r.UserId
                }).ToList(),
            };
            return restaurantDTO;
        }




        public async Task<IEnumerable<GetRestaurantDTO>> GetRestaurantsAsync()
        {
            var restaurants = await _dbContext.Restaurants
                .Include(r => r.Category)
                .Include(r => r.Rating)
                .Include(r => r.Comment)
                .ThenInclude(r => r.CommentChildren)
                .Include(r => r.Menus)
                .Include(r => r.RestaurantImages)
                .Include(r => r.Bookings)
                .ThenInclude(r => r.User)
                .ToListAsync();


            Booking booking = null;
            List<Menu> menus = null;
            decimal total = 0;
            if(restaurants.Count > 0)
            {
                foreach (var restaurant in restaurants)
                {
                    booking = restaurant.Bookings.FirstOrDefault();
                    if(booking != null && booking.MenuId != null)
                    {
                        menus = await _dbContext.Menus.Where(m => booking.MenuId.Contains(m.Id.ToString())).ToListAsync();
                    }
                }
            }
            if(menus != null)
            {
                foreach (var menu in menus)
                {
                    total = (menu.Price * menu.Quantity) + total;
                }
            }


            var restaurantDTO = restaurants.Select(r => new GetRestaurantDTO
            {
                Id = r.Id,
                RestaurantName = r.RestaurantName,
                City = r.City,
                UserId = (int)r.UserId,
                Address = r.Address,
                Open = r.Open,
                Close = r.Close,
                DisCount = r.DisCount,
                Status = r.Status,
                Warning = (int)r.Warning,
                TotalRating = r.TotalRating,
                Booking = r.Bookings != null ? r.Bookings.Select(u => new BookingDTO
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
                    User = new UserDTO
                    {
                        Id = u.User.Id,
                        UserEmail = u.User.UserEmail,
                        UserName = u.User.UserName,
                        Phone = u.User.Phone
                    },
                    Menu = menus?.Select(m => new GetMenuDTO
                    {
                        Id = m.Id,
                        MenuImage = m.MenuImage,
                        MenuName = m.MenuName,
                        Price = m.Price,
                        Quantity = m.Quantity
                    }).ToList(),

                }).ToList() : null,
                Menus = r.Menus.Select(m => new GetMenuDTO
                {
                    Id = m.Id,
                    MenuName = m.MenuName,
                    Price = m.Price,
                    Ingredient = m.Ingredient,
                    MenuImage = m.MenuImage,
                }).ToList(),
                RestaurantImages = r.RestaurantImages.Select(m => new RestaurantImagesDTO
                {
                    Id = m.Id,
                    ImagesUrl = m.ImagesUrl,
                }).ToList(),
                Comment = r.Comment.Select(c => new CommentDTO
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    Content = c.Content,
                    CommentChildren = c.CommentChildren.Select(x => new CommentChildDTO
                    {
                        Id = x.Id,
                        CommentId = x.CommentId,
                        UserId = x.Id,
                        Content = x.Content,
                    }).ToList(),
                }).ToList(),
                Category = new CategoryDTO
                {
                    Title = r.Category.Title
                },
                Rating = r.Rating.Select(r => new RatingDTO
                {
                    Id = r.Id,
                    Point = r.Point,
                    UserId = r.UserId
                }).ToList(),
                
            }).ToList();
            return restaurantDTO;
        }

        public async Task UpdateRestaurantAsync(Restaurant restaurant)
        {
            
            _dbContext.Restaurants.Update(restaurant);
            await _dbContext.SaveChangesAsync();
        }
    }
}
