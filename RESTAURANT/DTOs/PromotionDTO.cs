using System.ComponentModel.DataAnnotations.Schema;

namespace RESTAURANT.API.DTOs
{
    public class PromotionDTO
    {
        public int Id { get; set; }

        public int? OrderId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImagePath { get; set; }

        public IFormFile? ImageFile { get; set; }

        public bool Status { get; set; } = false;
        public int QuantityTable { get; set; }
        public decimal Price { get; set; }

    }
}
