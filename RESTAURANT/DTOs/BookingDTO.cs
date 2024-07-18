namespace RESTAURANT.API.DTOs
{
    public class BookingDTO
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public int? RestaurantId { get; set; }
        public int Member { get; set; }
        public DateTime DayArrive { get; set; }
        public DateTime Hour { get; set; }
        public bool Status { get; set; } = false;
        public List<string>? MenuId { get; set; }
        public int? Pont { get; set; }
        public decimal Total { get; set; }
        public string? Description { get; set; }
        public UserDTO? User { get; set; }
        public List<GetMenuDTO>? Menu { get; set; }
    }
}
