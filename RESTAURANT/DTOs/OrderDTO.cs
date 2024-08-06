using RESTAURANT.API.Models;

namespace RESTAURANT.API.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int? UserId { get; set; }

        public int? DishId { get; set; }
        public int? ComboId { get; set; }

        public int? CustomComboId { get; set; }
        public decimal TotalPrice { get; set; }
        public int QuantityTable { get; set; }
        public bool? StatusPayment { get; set; }
        public decimal Deposit { get; set; }
        public string Oganization { get; set; }
        
        public int PromotionId { get; set; }
        public int? LobbyId { get; set; }
        public UserDTO? User { get; set; }

        public ComboDTO? Combo { get; set; }
        public bool? Status { get; set; }
        public LobbyDTO? Lobby { get; set; }

        public List<GetOrderAppetizerDTO>? GetOrderAppetizers { get; set; }
        public List<GetOrderDessertDTO>? GetOrderDesserts { get; set; }
        public List<GetOrderDishDTO>? GetOrderDishes { get; set; }

    }
}
