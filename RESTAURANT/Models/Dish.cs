using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RESTAURANT.API.Models
{
    public class Dish
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public bool Status { get; set; }

        public ICollection<ComboDish>? ComboDishes { get; set; }

        public int CustomComboId { get; set; }

        public CustomCombo? CustomCombo { get; set; }


        public string? ImagePath { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
