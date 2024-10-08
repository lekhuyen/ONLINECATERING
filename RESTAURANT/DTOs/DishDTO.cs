﻿namespace RESTAURANT.API.DTOs
{
    public class DishDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool? Status { get; set; }
        public string? Image { get; set; }
        public decimal? TotalRating { get; set; }
        public IFormFile? ImageFile { get; set; }
        public List<CommentDTO>? Comments { get; set; }
        public List<RatingDTO>? Ratings { get; set; }
        public int? CountRatings { get; set; }
    }
}
