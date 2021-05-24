using System.ComponentModel.DataAnnotations;

namespace MyBlazorApp.ViewModel.Products
{
    public class CreateProductsViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]        
        public double Price { get; set; }
    }
}