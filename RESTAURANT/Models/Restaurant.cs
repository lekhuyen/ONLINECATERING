using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RESTAURANT.API.Models
{
    public class Restaurant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string RestaurantName { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public DateTime Open { get; set; }
        public DateTime Close { get; set; }
        public int? DisCount { get; set; }
        public int? Point { get; set; }
        //open/close
        public bool Status { get; set; } = false;

        //canh bao khi bi bao cao xau
        public int? Warning { get; set; } = 0;
        public int CategoryId { get; set; }
        public decimal? TotalRating { get; set; }
        public int? Menu { get; set; }
        //UserId => user created restaurant
        public int? UserId { get; set; }

        public Category? Category { get; set; }
        //public int? RatingId { get; set; }
       
        public ICollection<Rating>? Rating { get; set; }
        public ICollection<Menu>? Menus { get; set; }
        public ICollection<Comment>? Comment { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
        public ICollection<Description>? Descriptions { get; set; }

        public ICollection<RestaurantImages>? RestaurantImages { get; set; }
    }
}
