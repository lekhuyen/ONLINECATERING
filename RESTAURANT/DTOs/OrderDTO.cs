namespace RESTAURANT.API.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public int QuantityTable { get; set; }
        public bool StatusPayment { get; set; }
        public decimal Deposit { get; set; }
        public DateTime Oganization { get; set; }

    }
}
