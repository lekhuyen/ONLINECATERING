using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace USER.API.Models
{
    public class Menu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string MenuName { get; set; }
        public string? Ingredient { get; set; }
        public decimal Price { get; set; }
        public int Quatity { get; set; }
        public int RestaurantId { get; set; }
        public string? MenuImage { get; set; }
        //public ICollection<MenuBooking>? MenuBookings { get; set; }
        public int? BookingId { get; set; }
        //public Booking? Booking { get; set; }
    }
}
