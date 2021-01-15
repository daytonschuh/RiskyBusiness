using ModelLayer.Models;
using System.Collections.Generic;

namespace ModelLayer.ViewModels
{
    public class CartViewModel
    {
        public Product Product { get; set; }
        public List<CartItem> CartItems { get; set; }
        public decimal Total { get; set; }
    }
}
