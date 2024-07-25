using RESTAURANT.API.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RESTAURANT.API.DTOs
{
    public class OrderDishDTO
    {

        public int OrderDishId { get; set; }
        public int DishId { get; set; }
        public int OrderId { get; set; }
        public OrderDTO Order { get; set; }
        public DishDTO Dish { get; set; }
    }
}
