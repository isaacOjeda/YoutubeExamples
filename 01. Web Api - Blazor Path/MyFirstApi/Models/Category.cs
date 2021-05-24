using System.Collections.Generic;

namespace MyFirstApi.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; } =
            new HashSet<Product>();
    }
}