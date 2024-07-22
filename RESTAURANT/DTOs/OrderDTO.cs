namespace RESTAURANT.API.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
/*        public UserDTO? User { get; set; }
        public CustomComboDTO? CustomCombo { get; set; }*/
/*        public ICollection<PromotionDTO>? Promotions { get; set; }
*/
        public int PromotionId { get; set; }  // Property to hold Promotion Id

        public decimal TotalPrice { get; set; }
        public int QuantityTable { get; set; }
        public bool StatusPayment { get; set; }
        public decimal Deposit { get; set; }
        public DateTime Oganization { get; set; }
/*        public PaymentDTO? Payment { get; set; }*/
    }
}
