using RESTAURANT.API.Models;

namespace RESTAURANT.API.DTOs
{
    public class RatingDTO
    {
        public int? Id { get; set; }
        public int Point { get; set; }
        public int UserId { get; set; }
        public int? RestaurantId { get; set; }
        public int? AppetizerId { get; set; }
        public int? DishId { get; set; }
        public int? DessertId { get; set; }
        public UserDTO? User { get; set; }
    }
}
