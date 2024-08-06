namespace RESTAURANT.API.DTOs
{
    public class CreateOrderDTO
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public int? ComboId { get; set; }
        public int? LobbyId { get; set; }
        public decimal TotalPrice { get; set; }
        public int QuantityTable { get; set; }
        public decimal Deposit { get; set; }
        public string Oganization { get; set; }

        public List<OOderDishDTO>? OrderDish { get; set; }
        public List<OrderAppetizerDTO>? OrderAppetizer { get; set; }
        public List<OrderDessertDTO>? OrderDessert { get; set; }

    }
}
