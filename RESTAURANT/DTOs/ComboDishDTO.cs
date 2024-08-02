using RESTAURANT.API.DTOs;

public class ComboDishDTO
{
    // Dish properties
    public int DishId { get; set; }
    public string? DishName { get; set; }
    public decimal? DishPrice { get; set; }
    public bool? DishStatus { get; set; }
    public string? DishImagePath { get; set; }
    public IFormFile? DishImageFile { get; set; }

    // Combo properties
    public int ComboId { get; set; }
    public string? ComboName { get; set; }
    public decimal? ComboPrice { get; set; }
    public bool? ComboStatus { get; set; }
    public int? ComboType { get; set; }
    public string? ComboImagePath { get; set; }
    public IFormFile? ComboImageFile { get; set; }
}