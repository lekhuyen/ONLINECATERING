namespace RESTAURANT.API.DTOs
{
    public class ComboDishDTO
    {
        public int DishId { get; set; }
        public DishDTO? Dish { get; set; }
        public int ComboId { get; set; }
        public ComboDTO? Combo { get; set; }
    }
}
