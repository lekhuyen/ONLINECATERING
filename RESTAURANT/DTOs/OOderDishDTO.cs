using RESTAURANT.API.Models;

namespace RESTAURANT.API.DTOs
{
    public class OOderDishDTO
    {
        public int? OrderId { get; set; }
        public int DishId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
