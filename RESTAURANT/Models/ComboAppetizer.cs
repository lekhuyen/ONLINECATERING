﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RESTAURANT.API.Models
{
    public class ComboAppetizer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? ComboId { get; set; }
        public int? AppetizerId { get; set; }
        public Combo? Combo { get; set; }
        public Appetizer? Appetizer { get; set; }
        public int? Quantity { get; set; } = 1;
    }
}
