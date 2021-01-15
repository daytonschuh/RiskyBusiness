using P1_DaytonSchuh.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLayer.Models
{
    public class ShoppingCart
    {
        // default constructor
        public ShoppingCart() { }

        // overloaded constructor with customer id set
        public ShoppingCart(string customerId) { CustomerId = customerId; }

        [Key]
        public int ShoppingCartId { get; set; }
        // [ForeignKey("AppUser")]
        public string CustomerId { get; set; }
    }
}
