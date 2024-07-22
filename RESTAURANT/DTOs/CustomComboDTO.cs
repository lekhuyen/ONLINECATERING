namespace RESTAURANT.API.DTOs
{
    public class CustomComboDTO
    {
        public int Id { get; set; }
        public DishDTO? Dish { get; set; }
        public UserDTO? User { get; set; }
        public DateTime Date { get; set; }
        public OrderDTO? Order { get; set; }
    }
}
