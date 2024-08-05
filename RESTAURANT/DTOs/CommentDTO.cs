using RESTAURANT.API.Models;

namespace RESTAURANT.API.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? DishId { get; set; }
        public int? DessertId { get; set; }
        public string Content { get; set; }
        public int? AppetizerId { get; set; }
        public int? BeverageId { get; set; }
        public List<CommentChildDTO>? CommentChildren { get; set; }
        public UserDTO? User { get; set; }
    }
}
