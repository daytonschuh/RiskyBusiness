using P1_DaytonSchuh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.ViewModels
{
    public class CheckoutViewModel
    {
        List<ProductViewModel> ProductViewModels { get; set; }
        Location Location { get; set; }
        public string CustomerId { get; set; }
    }
}
