using P1_DaytonSchuh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.ViewModels
{
    public class AddToOrderViewModel
    {
        public ProductViewModel ProductViewModel { get; set; }
        public int Quantity { get; set; }
    }
}
