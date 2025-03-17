using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using ECommerce.Data;


namespace ECommerce.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        
        [Column("order_date")]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        
        public decimal Total { get; set; }
        
        public string Status { get; set; } = "pending";
        
        [Column("shipping_address")]
        public string? ShippingAddress { get; set; }

        // Relazione con l'entità ApplicationUser
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
