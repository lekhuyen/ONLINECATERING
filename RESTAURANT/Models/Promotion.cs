using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RESTAURANT.API.Models
{
    public class Promotion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)] // Example of specifying max length
        public string Name { get; set; }

        public string Description { get; set; }

        public int Status { get; set; } // Consider renaming 'status' to 'Status'

        public int QuantityTable { get; set; }

        [Column(TypeName = "decimal(10,2)")] // Example of specifying column type
        public decimal Price { get; set; }

        public ICollection<Order>? Orders { get; set; }

    }
}
