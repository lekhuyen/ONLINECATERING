using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RESTAURANT.API.Models
{
    public class CommentChild
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public Comment? Comment { get; set; }
        public User? User { get; set; }
    }
}
