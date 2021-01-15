using System.ComponentModel.DataAnnotations;

namespace ModelLayer.Models
{
    public class CartItem
    {
        public CartItem(){ }

        [Key]
        public int ItemId { get; set; }

        public int CartId { get; set; }

        public int Quantity { get; set; }

        public int ProductId { get; set; }
    }
}
