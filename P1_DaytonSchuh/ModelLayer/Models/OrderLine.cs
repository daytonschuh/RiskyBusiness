using ModelLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace P1_DaytonSchuh.Models
{
    public class OrderLine
    {
        public OrderLine() { }
        public OrderLine(int orderId, string customerId, int prodId, decimal price, int quantity)
        {
            this.OrderId = orderId;
            this.CustomerId = customerId;
            this.ProductId = prodId;
            this.Price = price;
            this.Quantity = quantity;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        // [ForeignKey("CustomerId")]
        public AppUser Customer { get; set; }

        // [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
