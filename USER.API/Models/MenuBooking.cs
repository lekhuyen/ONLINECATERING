using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace USER.API.Models
{
    public class MenuBooking
    {
        public int MenuId { get; set; }
        public int BookingId { get; set; }
        public Menu? Menu { get; set; }
        public Booking? Booking { get; set; }
    }
}
