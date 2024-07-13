﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RESTAURANT.API.Models
{
    public class Rating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Point { get; set; }
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }
        
    }
}
