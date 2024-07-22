using RESTAURANT.API.DTOs;

public class ComboDishDTO
{
    public int DishId { get; set; }
    public DishDTO? Dish { get; set; }
    public int ComboId { get; set; }
    public ComboDTO? Combo { get; set; }

/*    public string DishName { get; set; }
    public decimal DishPrice { get; set; } // Change to decimal
    public string ComboName { get; set; }
    public decimal ComboPrice { get; set; } // Change to decimal*/
}