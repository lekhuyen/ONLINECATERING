namespace RESTAURANT.API.DTOs
{
    public class MessageDTO
    {
        public int? Id { get; set; }
        public string message { get; set; }
        public int UserId { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string Roomname { get; set; }
        public string Username { get; set; }
    }
}
