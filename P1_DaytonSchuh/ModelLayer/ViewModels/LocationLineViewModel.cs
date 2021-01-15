using ModelLayer.Models;
using System.ComponentModel.DataAnnotations;

namespace ModelLayer.ViewModels
{
    public class LocationLineViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int LocationId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        public Product product { get; set; }
    }
}
