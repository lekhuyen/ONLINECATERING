﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RESTAURANT.API.Models
{
    public class Appetizer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string AppetizerName { get; set; }
        public decimal Price { get; set; }
        public int? Quantity { get; set; }
        public string? AppetizerImage { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<ComboAppetizer>? ComboAppetizers { get; set; }
        public ICollection<Rating>? Rating { get; set; }
        public decimal? TotalRating { get; set; }
        public int? CountRatings { get; set; }
        public ICollection<OrderAppetizer>? OrderAppetizer { get; set; }
    }
}
