﻿namespace RESTAURANT.API.DTOs
{
    public class DessertDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int? Quantity { get; set; }
        public string? Image { get; set; }
        public List<CommentDTO>? Comments { get; set; }
        public List<RatingDTO>? Ratings { get; set; }
        public decimal? TotalRating { get; set; }
        public int? CountRatings { get; set; }
    }
}
