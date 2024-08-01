using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RESTAURANT.API.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Phone { get; set; }
        public int? BookingId { get; set; }
        public ICollection<Booking>? Booking { get; set; }

        public ICollection<Order>? Orders { get; set; }
        public ICollection<CustomCombo>? CustomCombos { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<CommentChild>? CommentChildren { get; set; }


    }
}
