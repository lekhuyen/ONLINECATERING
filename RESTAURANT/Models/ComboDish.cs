namespace RESTAURANT.API.Models
{
    public class ComboDish
    {
        public int DishId { get; set; }
        public Dish? Dish { get; set; }
        public int ComboId { get; set; }
        public Combo? Combo { get; set; }

    }
}
