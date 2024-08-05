using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace USER.API.Models
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string message { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        public string Roomname { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
