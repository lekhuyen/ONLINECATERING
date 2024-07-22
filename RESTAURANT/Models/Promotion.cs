using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RESTAURANT.API.Models
{
    public class Promotion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string? ImagePath { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public bool Status { get; set; }
        public int QuantityTable { get; set; }
        public decimal Price { get; set; }
        public Order? Orders { get; set; }

    }
}
