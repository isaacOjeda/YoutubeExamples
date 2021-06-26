using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MyFirstApi.Dto.Products
{
    public class ProductCreateEditDto
    {
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public double? Price { get; set; }

        [Required]
        public int? CategoryId { get; set; }

        public string CategoryName { get; set; }

        public IFormFile Photo { get; set; }
    }
}