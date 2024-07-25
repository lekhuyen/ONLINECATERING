using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RESTAURANT.API.Models
{
    public class OrderDish
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDishId { get; set; }

        public int DishId { get; set; }

        public int OrderId { get; set; }

        public Order? Order { get; set; }

        public Dish? Dish { get; set; }
    }
}
