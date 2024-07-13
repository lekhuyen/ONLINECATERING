using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace USER.API.Models
{
    public class FavoriteList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string RestaurantName { get; set; }
        public string? Image { get; set; }
        public string Address { get; set; }
        public decimal Rating { get; set; }
        public User? User { get; set; }
    }
}
