using System;
using System.Net.Http;
using System.Threading.Tasks;
using MyFirstBlazorApp;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();
            var apiClient = new ProductsClient(httpClient);

            var products = await apiClient.GetProductsAsync();

            foreach (var product in products)
            {
                Console.WriteLine($"Name {product.Name}");
                Console.WriteLine($"Description {product.Description}");
                Console.WriteLine($"Price {product.Price:c}");
            }
        }
    }
}
