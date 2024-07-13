using RESTAURANT.API.DTOs;
using RESTAURANT.API.Models;

namespace RESTAURANT.API.Repositories
{
    public interface IRestaurant
    {
        Task<IEnumerable<GetRestaurantDTO>> GetRestaurantsAsync();
        Task<PostRestaurantDTO> AddRestaurantAsync(PostRestaurantDTO restaurant);
        Task UpdateRestaurantAsync(Restaurant restaurant);
        Task DeleteRestaurantAsync(int id);
        Task<Restaurant> GetRestaurantByIdAsync(int id);
        Task<GetRestaurantDTO> GetOneRestaurantByIdAsync(int id);

    }
}
