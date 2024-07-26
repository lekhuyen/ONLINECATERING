using RESTAURANT.API.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace RESTAURANT.API.DTOs
{
    public class ComboAppetizerDTO
    {
        public int? AppetizerId { get; set; }
        public string AppetizerName { get; set; }
        public decimal AppetizerPrice { get; set; }
        public int? AppetizerQuantity { get; set; }
        public string? AppetizerImage { get; set; }

        public int? ComboId { get; set; }
        public string ComboName { get; set; }
        public decimal ComboPrice { get; set; }
        public bool? Status { get; set; }
        public string? ComboImagePath { get; set; }
        public int? ComboType { get; set; }
    }
}
