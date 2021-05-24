using System.ComponentModel.DataAnnotations;

namespace MyFirstApi.Dto.Products
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]        
        public double? Price { get; set; }
    }
}