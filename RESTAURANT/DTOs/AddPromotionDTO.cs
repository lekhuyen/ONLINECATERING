namespace RESTAURANT.API.DTOs
{
    public class AddPromotionDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public bool Status { get; set; }
        public int QuantityTable { get; set; }
        public decimal Price { get; set; }
    }
}
