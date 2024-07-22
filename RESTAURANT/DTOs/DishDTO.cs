namespace RESTAURANT.API.DTOs
{
    public class DishDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool Status { get; set; }
        public string? ImagePath { get; set; }

        public IFormFile ImageFile { get; set; }

        public ICollection<ComboDishDTO>? ComboDishes { get; set; }
        public CustomComboDTO? CustomCombo { get; set; }
    }
}
