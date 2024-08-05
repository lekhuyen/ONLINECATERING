using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RESTAURANT.API.Models
{
    public class ComboBeverage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? ComboId { get; set; }
        public int? BeverageId { get; set; }
        public Combo? Combo { get; set; }
        public Beverage? Beverage { get; set; }
    }
}
