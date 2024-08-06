namespace RESTAURANT.API.DTOs
{
    public class GetOrderDessertDTO
    {
        public int? OrderId { get; set; }
        public int DessertId { get; set; }
        public int Quantity { get; set; } = 1;
        public DessertDTO? Dessert { get; set; }
    }
}
