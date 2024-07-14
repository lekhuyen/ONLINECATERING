using RESTAURANT.API.Models;

namespace RESTAURANT.API.DTOs
{
    public class RestaurantImagesDTO
    {
        public int Id { get; set; }
        public string? ImagesUrl { get; set; }
        public int? RestaurantId { get; set; }
    }
}
