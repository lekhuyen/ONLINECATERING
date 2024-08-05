using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RESTAURANT.API.Models
{
    public class Combo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        [Column(TypeName = "decimal(10,2)")] // Example of specifying column type
        public decimal Price { get; set; }

        public bool Status { get; set; }

        public int Type {  get; set; }

        public ICollection<ComboDish>? ComboDishes { get; set; }

        public string? ImagePath { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public ICollection<ComboAppetizer>? ComboAppetizers { get; set; }
        public ICollection<ComboDessert>? ComboDesserts { get; set; }

        public ICollection<Promotion>? Promotions { get; set; }

        public ICollection<Order>? Order { get; set; }

        public ICollection<ComboBeverage>? ComboBeverages { get; set; }

    }
}
