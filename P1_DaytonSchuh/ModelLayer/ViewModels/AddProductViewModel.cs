using System.ComponentModel.DataAnnotations;

namespace ModelLayer.ViewModels
{
    public class AddProductViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Display(Name = "Image")]
        public byte[] ProductPicture { get; set; }
    }
}
