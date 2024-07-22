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

        [Column(TypeName = "decimal(10,2)")] // Example of specifying column type
        public decimal TotalPrice { get; set; }

        public int QuantityTable { get; set; }

        public bool StatusPayment { get; set; }

        [Column(TypeName = "decimal(10,2)")] // Example of specifying column type
        public decimal Deposit { get; set; }

        public DateTime Organization { get; set; }

        public int UserId { get; set; }  // Foreign key to User table

        public ICollection<User>? User { get; set; }

        public ICollection<Promotion>? Promotions { get; set; }  // Collection navigation property to Promotion table


        public Payment? Payment { get; set; }  // One-to-one relationship with Payment


        public CustomCombo? CustomCombo { get; set; }  // One-to-one relationship with CustomCombo


    }
}
