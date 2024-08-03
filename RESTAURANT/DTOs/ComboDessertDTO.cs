namespace RESTAURANT.API.DTOs
{
    public class ComboDessertDTO
    {
        public int ComboDessertId { get; set; }
        public int? DessertId { get; set; }
        public string DessertName { get; set; }
        public decimal DessertPrice { get; set; }
        public int? DessertQuantity { get; set; }
        public string? DessertImage { get; set; }


        public int? ComboId { get; set; }
        public string ComboName { get; set; }
        public decimal ComboPrice { get; set; }
        public bool? Status { get; set; }
        public string? ComboImagePath { get; set; }
        public int? ComboType { get; set; }
    }
}
