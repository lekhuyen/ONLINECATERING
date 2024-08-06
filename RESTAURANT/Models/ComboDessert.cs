using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RESTAURANT.API.Models
{
    public class ComboDessert
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? ComboId { get; set; }
        public int? DessertId { get; set; }
        public Combo? Combo { get; set; }
        public Dessert? Dessert { get; set; }
        public int? Quantity { get; set; } = 1;
    }
}
