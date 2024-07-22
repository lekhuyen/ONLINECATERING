using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RESTAURANT.API.Models
{
    public class CustomCombo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int DishId { get; set; }  // Foreign key to Dish tabl

        public Dish? Dish { get; set; }  // Navigation property to Dish table

        public int UserId { get; set; }  // Foreign key to User table

        public User? User { get; set; }   // Navigation property to User table

        public Order? Order { get; set; }

        public DateTime Date { get; set; }
    }

}
