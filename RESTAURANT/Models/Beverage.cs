using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RESTAURANT.API.Models
{
    public class Beverage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string BeverageName { get; set; }
        public decimal Price { get; set; }
        public int? Quantity { get; set; }
        public string? BeverageImage { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<ComboBeverage>? ComboBeverages { get; set; }
        public ICollection<Rating>? Rating { get; set; }
        public decimal? TotalRating { get; set; }
        public int? CountRatings { get; set; }
    }
}
