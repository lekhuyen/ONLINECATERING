﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RESTAURANT.API.Models
{
    public class Dessert
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string DessertName { get; set; }
        public decimal Price { get; set; }
        public int? Quantity { get; set; }
        public string? DessertImage { get; set; }
        public ICollection<ComboDessert>? ComboDesserts { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Rating>? Rating { get; set; }
        public decimal? TotalRating { get; set; }
        public int? CountRatings { get; set; }
        public ICollection<OrderDessert>? OrderDesserts { get; set; }
    }
}
