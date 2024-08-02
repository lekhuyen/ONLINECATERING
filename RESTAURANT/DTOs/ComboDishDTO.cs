using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RESTAURANT.API.DTOs
{
    public class ComboDishDTO
    {
        public int ComboDishId { get; set; }

        // Dish properties
        public int DishId { get; set; }
        public string DishName { get; set; }
        public decimal DishPrice { get; set; }
        public string DishImagePath { get; set; }


        // Combo properties
        public int ComboId { get; set; }
        public string ComboName { get; set; }
        public decimal ComboPrice { get; set; }
        public int ComboType { get; set; }
        public string ComboImagePath { get; set; }
    }
}
