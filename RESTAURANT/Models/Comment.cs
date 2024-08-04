using RESTAURANT.API.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RESTAURANT.API.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public int? RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }
        public int? DishId { get; set; }
        public Dish? Dish { get; set; }
        public ICollection<CommentChild>? CommentChildren { get; set; }
        public bool? Status { get; set; } = false;
        public int? AppetizerId { get; set; }
        public Appetizer? Appetizer { get; set; }
        public int? DessertId { get; set; }
        public Dessert? Dessert { get; set; }
        public User? User { get; set; }
    }
}
