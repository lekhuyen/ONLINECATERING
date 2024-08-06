using RESTAURANT.API.Models;

namespace RESTAURANT.API.DTOs
{
    public class OrderDessertDTO
    {
        public int? OrderId { get; set; }
        public int DessertId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
