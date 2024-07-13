using RESTAURANT.API.Models;

namespace RESTAURANT.API.DTOs
{
    public class GetRestaurantDTO
    {
        public int? Id { get; set; }
        public string RestaurantName { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public int UserId { get; set; }
        public DateTime Open { get; set; }
        public DateTime Close { get; set; }
        public int? DisCount { get; set; }
        //open/close
        public bool Status { get; set; }
        public decimal? TotalRating { get; set; }

        //canh bao khi bi bao cao xau
        public int Warning { get; set; }


        public CategoryDTO? Category { get; set; }
        public List<RatingDTO>? Rating { get; set; }

        public List<CommentDTO>? Comment { get; set; }
        public List<RestaurantImagesDTO>? RestaurantImages { get; set; }
    }
}
