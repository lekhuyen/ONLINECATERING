﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RESTAURANT.API.Models
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public decimal TotalPrice { get; set; }
        public string Method { get; set; }
        public DateTime Date { get; set; }
        public int Type { get; set; }

    }
}
