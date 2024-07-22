namespace RESTAURANT.API.DTOs
{
    public class UserDTO
    {
        public int? Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Phone { get; set; }

        public ICollection<OrderDTO>? Orders { get; set; }
        public CustomComboDTO? CustomCombo { get; set; }
    }
}
