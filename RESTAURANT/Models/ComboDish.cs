using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RESTAURANT.API.Models
{
    public class ComboDish
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int DishId { get; set; }
        public Dish? Dish { get; set; }
        public int ComboId { get; set; }
        public Combo? Combo { get; set; }

    }
}
