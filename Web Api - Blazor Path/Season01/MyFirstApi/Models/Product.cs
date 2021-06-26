namespace MyFirstApi.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string PhotoName { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
    }
}