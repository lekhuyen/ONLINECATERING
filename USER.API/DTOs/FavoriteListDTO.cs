namespace USER.API.DTOs
{
    public class FavoriteListDTO
    {
        public string RestaurantName { get; set; }
        public int? UserId { get; set; }
        public string? Image { get; set; }
        public string Address { get; set; }
        public decimal Rating { get; set; }
    }
}
