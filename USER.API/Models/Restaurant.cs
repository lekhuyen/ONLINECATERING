using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace USER.API.Models
{
    public class Restaurant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string RestaurantName { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public DateTime? Open { get; set; }
        public DateTime? Close { get; set; }
        public int? Category { get; set; }
        public int? UserId { get; set; }
        public int? BookingId { get; set; }
        public Booking? Booking { get; set; }
        //public User? User { get; set; }
    }
}
