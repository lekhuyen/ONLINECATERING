namespace RESTAURANT.API.DTOs
{
    public class CustomComboDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? Phone { get; set; }

        public int DishId { get; set; }
        public string? DishName { get; set; }
        public decimal? DishPrice { get; set; }
        public bool? DishStatus { get; set; }
        public string? DishImagePath { get; set; }

        public int? OrderId { get; set; }


        public DateTime Date { get; set; }

    }
}
