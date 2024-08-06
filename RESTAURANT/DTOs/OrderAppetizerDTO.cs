using RESTAURANT.API.Models;

namespace RESTAURANT.API.DTOs
{
    public class OrderAppetizerDTO
    {
        public int? OrderId { get; set; }
        public int AppetizerId { get; set; }
        public int Quantity { get; set; } = 1;
        List<AppetizerDTO>? Appetizers { get; set; }
    }
}
