using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RESTAURANT.API.Models
{
    public class OrderDish
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDishId { get; set; }

        [Column(Order = 1)]
        public int DishId { get; set; }

        [Column(Order = 0)]
        public int OrderId { get; set; }


        public Order? Order { get; set; }

        public Dish? Dish { get; set; }
    }
}
