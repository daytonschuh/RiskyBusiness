using ModelLayer.Models;
using P1_DaytonSchuh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Data.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> Products { get; set; }
        IEnumerable<Product> PreferredProducts { get; set; }
        Product GetProductById(int id);
    }
}
