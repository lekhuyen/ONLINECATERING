using RESTAURANT.API.Models;

namespace RESTAURANT.API.DTOs
{
    public class PostRestaurantDTO
    {
        public int? Id { get; set; }
        public string RestaurantName { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public DateTime Open { get; set; }
        public DateTime Close { get; set; }
        public int? DisCount { get; set; }
        public int? CategoryId { get; set; }
        public int? UserId { get; set; }
        //open/close
        public bool Status { get; set; } = false;

        //canh bao khi bi bao cao xau
        public int Warning { get; set; } = 0;


        //public Category? Category { get; set; }
        //public List<Rating>? Rating { get; set; }

        //public ICollection<Comment>? Comment { get; set; }
    }
}
