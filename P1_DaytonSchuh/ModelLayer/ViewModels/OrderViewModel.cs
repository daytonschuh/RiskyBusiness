using ModelLayer.Models;
using P1_DaytonSchuh;
using P1_DaytonSchuh.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelLayer.ViewModels
{
    public class OrderViewModel
    {
        [Required]
        [Display(Name ="Order ID")]
        public int OrderId { get; set; }
        public List<OrderLine> OrderLines { get; set; }
        [Required]
        public AppUser User { get; set; }
        [Required]
        public Location Location {get;set;}
        public decimal Total { get; set; }
        [Display(Name="Order Date")]
        public DateTime OrderDate { get; set; }
        public List<Product> Products { get; set; }
    }
}
