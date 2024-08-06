namespace RESTAURANT.API.DTOs
{
    public class GetOrderDishDTO
    {
        public int? OrderId { get; set; }
        public int DishId { get; set; }
        public int Quantity { get; set; } = 1;
        public DishDTO? DishDTO { get; set; }
    }
}
