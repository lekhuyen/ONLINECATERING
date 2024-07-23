using RESTAURANT.API.DTOs;

public class ComboDishDTO
{
    public int DishId { get; set; }
    public string? DishName { get; set; }
    public decimal? DishPrice { get; set; } // Nullable decimal

    public int ComboId { get; set; }
    public string? ComboName { get; set; }
    public decimal? ComboPrice { get; set; } // Nullable decimal
}