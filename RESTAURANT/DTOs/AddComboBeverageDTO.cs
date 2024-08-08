namespace RESTAURANT.API.DTOs
{
    public class AddComboBeverageDTO
    {
        public int ComboBeverageId { get; set; }
        public int? ComboId { get; set; }
        public int? BeverageId { get; set; }
    }
}
