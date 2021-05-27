namespace MyFirstApi.Dto.Products
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double? Price { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string UrlPhoto { get; set; }
    }
}