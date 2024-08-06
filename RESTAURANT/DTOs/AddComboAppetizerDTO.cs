namespace RESTAURANT.API.DTOs
{
    public class AddComboAppetizerDTO
    {
        //public int? ComboAppetizerId { get; set; }
        public int ComboId { get; set; }
        public int AppetizerId { get; set; }
        public int? Quantity { get; set; }
    }
}
