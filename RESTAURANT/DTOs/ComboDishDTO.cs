namespace RESTAURANT.API.DTOs
{
    public class ComboDishDTO
    {

        /*public int DishId { get; set; }
        public DishDTO? Dish { get; set; }
        public int ComboId { get; set; }
        public ComboDTO? Combo { get; set; }*/

        public string DishName { get; set; }

        public string DishPrice { get; set; }

        public string ComboName { get; set; }

        public string ComboPrice { get; set; }

        public DateTime StartDate { get; set; }
    }
}
