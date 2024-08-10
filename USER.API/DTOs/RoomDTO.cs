namespace USER.API.DTOs
{
    public class RoomDTO
    {
        public int Id { get; set; }
        public string RoomCode { get; set; }
        public List<UserDTO>? Users { get; set; }
    }
}
