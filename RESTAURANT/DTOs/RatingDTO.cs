using RESTAURANT.API.Models;

namespace RESTAURANT.API.DTOs
{
    public class RatingDTO
    {
        public int? Id { get; set; }
        public int Point { get; set; }
        public int UserId { get; set; }
        public int? RestaurantId { get; set; }
    }
}
