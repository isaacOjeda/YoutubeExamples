using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstApi.Data;
using MyFirstApi.Dto.Products;
using MyFirstApi.Models;

namespace MyFirstApi.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class ProductsController : ApiController
    {
        private readonly MyFirstApiDbContext _context;
        private readonly IWebHostEnvironment _host;
        private readonly HttpContext _http;

        public ProductsController(MyFirstApiDbContext context, IWebHostEnvironment host, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _host = host;
            _http = contextAccessor.HttpContext;
        }

        [HttpGet]
        public List<ProductDto> GetProducts()
        {
            return _context.Products
                .Select(s => new ProductDto
                {
                    Description = s.Description,
                    Name = s.Name,
                    Price = s.Price,
                    ProductId = s.ProductId,
                    CategoryId = s.CategoryId,
                    CategoryName = s.Category.Name,
                    UrlPhoto = $"{_http.Request.Scheme}://{_http.Request.Host}/Images/{s.PhotoName}"
                })
                .ToList();
        }

        [HttpGet]
        [Route("{productId}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProduct(int productId)
        {
            var product = await _context.Products
                .Select(product => new ProductDto
                {
                    Description = product.Description,
                    Name = product.Name,
                    Price = product.Price,
                    ProductId = product.ProductId,
                    CategoryId = product.CategoryId,
                    CategoryName = product.Category.Name,
                    UrlPhoto = $"{_http.Request.Scheme}://{_http.Request.Host}/Images/{product.PhotoName}"
                })
                .FirstOrDefaultAsync(q => q.ProductId == productId);

            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateEditDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newProduct = new Product
            {
                Description = model.Description,
                Name = model.Name,
                Price = model.Price.Value,
                CategoryId = model.CategoryId.Value
            };

            // Guardar foto en Storage
            if (model.Photo is not null)
            {
                var fileName = System.IO.Path.GetRandomFileName();
                var extension = System.IO.Path.GetExtension(model.Photo.FileName);

                newProduct.PhotoName = $"{fileName}{extension}";

                var path = $"{_host.WebRootPath}/Images/{newProduct.PhotoName}";

                using var fileStream = System.IO.File.Create(path);

                await model.Photo.CopyToAsync(fileStream);
            }

            _context.Products.Add(newProduct);

            await _context.SaveChangesAsync();

            return Accepted();
        }

        [HttpDelete]
        [Route("{productId}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product is null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return Accepted();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductCreateEditDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _context.Products.FindAsync(model.ProductId);

            if (product is null)
            {
                return NotFound();
            }

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price.Value;
            product.CategoryId = model.CategoryId.Value;

            // Guardar foto en Storage
            if (model.Photo is not null)
            {
                var fileName = System.IO.Path.GetRandomFileName();
                var extension = System.IO.Path.GetExtension(model.Photo.FileName);

                product.PhotoName = $"{fileName}{extension}";

                var path = $"{_host.WebRootPath}/Images/{product.PhotoName}";

                using var fileStream = System.IO.File.Create(path);

                await model.Photo.CopyToAsync(fileStream);
            }

            await _context.SaveChangesAsync();

            return Accepted();
        }

    }
}