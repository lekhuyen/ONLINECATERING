using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RESTAURANT.API.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public int? CustomComboId { get; set; }
        public CustomCombo? CustomCombo { get; set; }


        public int? ComboId { get; set; }
        public Combo? Combo { get; set; }

        public int? PromotionId { get; set; }
        public Promotion? Promotion { get; set; }
        public decimal TotalPrice { get; set; }
        public int QuantityTable { get; set; }
        public bool StatusPayment { get; set; } = false;
        public decimal Deposit { get; set; }
        public DateTime Oganization { get; set; }
        public Payment? Payment { get; set; }

        public ICollection<OrderDish>? OrderDishes { get; set; }
    }

}
