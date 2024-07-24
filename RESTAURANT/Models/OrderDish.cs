namespace RESTAURANT.API.Models
{
    public class OrderDish
    {
        public int Id { get; set; }

        public int DishId { get; set; }

        public int OrderId { get; set; }

        public Order? Order { get; set; }

        public Dish? Dish { get; set; }
    }
}
