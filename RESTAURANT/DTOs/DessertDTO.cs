namespace RESTAURANT.API.DTOs
{
    public class DessertDTO
    {
        public int? Id { get; set; }
        public string DessertName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? DessertImage { get; set; }
    }
}
