using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RESTAURANT.API.Models
{
    public class Description
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }
        public string Title { get; set; }
    }
}
