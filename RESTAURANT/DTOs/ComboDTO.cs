namespace RESTAURANT.API.DTOs
{
    public class ComboDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool Status { get; set; }
        public string? ImagePath { get; set; }

        public IFormFile? ImageFile { get; set; }

        public int? Type { get; set; }
        public List<PromotionDTO>? Promotions { get; set; }
        public List<ComboAppetizerDTO>? ComboAppetizers { get; set; }
        public List<ComboDessertDTO>? ComboDesserts { get; set; }
        public List<ComboDishDTO>? ComboDishes { get; set; }
    }
}
