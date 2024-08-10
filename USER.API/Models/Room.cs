using System.ComponentModel.DataAnnotations.Schema;

namespace USER.API.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string RoomCode { get; set; }
        public ICollection<User>? Users { get; set; }
        public ICollection<Message>? Messages { get; set; }
    }
}
