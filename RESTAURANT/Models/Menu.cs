using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RESTAURANT.API.Models
{
    public class Menu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string MenuName { get; set; }
        public string? Ingredient { get; set; }
        public decimal Price { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }
        public List<MenuImages>? MenuImages { get; set; }
    }
}
