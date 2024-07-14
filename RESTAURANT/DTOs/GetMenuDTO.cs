namespace RESTAURANT.API.DTOs
{
    public class GetMenuDTO
    {
        public int Id { get; set; }
        public string MenuName { get; set; }
        public decimal Price { get; set; }
        public string? Ingredient { get; set; }
        public string? MenuImage { get; set; }
        //public List<MenuImages>? MenuImages { get; set; }
    }
}
