using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.DTOs;
using RESTAURANT.API.Models;

namespace RESTAURANT.API.Repositories
{
    public class RestaurantRepositories : IRestaurant
    {
        private readonly DatabaseContext _dbContext;
        public RestaurantRepositories(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
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
                .FirstOrDefaultAsync(x => x.Id == id);

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
                RestaurantImages = restaurant.RestaurantImages.Select(m => new RestaurantImagesDTO
                {
                    Id = m.Id,
                    ImagesUrl = m.ImagesUrl,
                }).ToList(),
                Comment = restaurant.Comment.Select(c => new CommentDTO
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
                .Include(r => r.RestaurantImages)
                .ToListAsync();
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
