namespace RESTAURANT.API.DTOs
{
    public class GetOrderAppetizerDTO
    {
        public int? OrderId { get; set; }
        public int AppetizerId { get; set; }
        public int Quantity { get; set; } = 1;
        public AppetizerDTO? Appetizer { get; set; }
    }
}
