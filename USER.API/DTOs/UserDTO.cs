namespace USER.API.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Phone { get; set; }
        public string? Role { get; set; }
        public string? Password { get; set; }
        public bool? Status { get; set; }
        public string? RefeshToken { get; set; }
        public string? AccessToken { get; set; }
        public string? Otp { get; set; }
        public DateTime? OtpExpired { get; set; }

        public GradeDTO? Grade { get; set; }
        public List<FavoriteListDTO>? FavoriteList { get; set; }
        public List<BookingDTO>? Booking { get; set; }
        public List<RestaurantDTO>? Restaurants { get; set; }
        

    }
}
