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

        public bool Status { get; set; } = false;

        public ICollection<ComboDish>? ComboDishes { get; set; }

        public ICollection<CustomCombo>? CustomCombos { get; set; }


        public string? ImagePath { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public ICollection<OrderDish>? OrderDishes { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Rating>? Rating { get; set; }
        public decimal? TotalRating { get; set; }
        public int? CountRatings { get; set; }
    }
}
