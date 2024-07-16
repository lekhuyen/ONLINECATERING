using RESTAURANT.API.Models;

namespace RESTAURANT.API.DTOs
{
    public class EditMenuDTO
    {
        public int? Id { get; set; }
        public string MenuName { get; set; }
        public decimal Price { get; set; }
        public int RestaurantId { get; set; }
        public string? Ingredient { get; set; }
        public string? MenuImage { get; set; }
        public int? Quatity { get; set; }

        //public List<MenuImages>? MenuImages { get; set; }
    }
}
