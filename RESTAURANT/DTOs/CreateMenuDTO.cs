using RESTAURANT.API.Models;

namespace RESTAURANT.API.DTOs
{
    public class CreateMenuDTO
    {
        public string MenuName { get; set; }
        public decimal Price { get; set; }
        public int RestaurantId { get; set; }
        public string? Ingredient { get; set; }
        //public List<MenuImages>? MenuImages { get; set; }
    }
}
