namespace RESTAURANT.API.DTOs
{
    public class UpdateOrderDishDTO
    {
        public int OrderDishId { get; set; }
        public int OrderId { get; set; }
        public int DishId { get; set; }
    }
}
