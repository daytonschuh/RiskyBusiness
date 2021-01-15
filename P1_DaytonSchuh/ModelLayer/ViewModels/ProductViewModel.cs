using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ModelLayer.ViewModels
{
    public class ProductViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        //[Required]
        [Display(Name = "Image")]
        public byte[] ProductPicture { get; set; }
        public IFormFile IformFileImage { get; set; }
        public int Quantity { get; set; }
    }
}
