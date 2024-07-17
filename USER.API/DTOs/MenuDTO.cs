namespace USER.API.DTOs
{
    public class MenuDTO
    {
        public int Id { get; set; }
        public string MenuName { get; set; }
        public string? Ingredient { get; set; }
        public decimal Price { get; set; }
        public int Quatity { get; set; }
        public int? RestaurantId { get; set; }
        public string? MenuImage { get; set; }
        public int? BookingId { get; set; }
    }
}
