﻿namespace RESTAURANT.API.DTOs
{
    public class PaymentDTO
    {
        public int Id { get; set; }
        public OrderDTO? Order { get; set; }
        public decimal TotalPrice { get; set; }
        public string Method { get; set; }
        public DateTime Date { get; set; }
        public int Type { get; set; }
    }
}
