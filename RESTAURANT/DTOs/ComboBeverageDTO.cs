namespace RESTAURANT.API.DTOs
{
    public class ComboBeverageDTO
    {
        public int ComboBeverageId { get; set; }
        public int? BeverageId { get; set; }
        public string BeverageName { get; set; }
        public decimal BeveragePrice { get; set; }
        public int? BeverageQuantity { get; set; }
        public string? BeverageImage { get; set; }

        public int? ComboId { get; set; }
        public string ComboName { get; set; }
        public decimal ComboPrice { get; set; }
        public bool? Status { get; set; }
        public string? ComboImagePath { get; set; }
        public int? ComboType { get; set; }
    }
}
