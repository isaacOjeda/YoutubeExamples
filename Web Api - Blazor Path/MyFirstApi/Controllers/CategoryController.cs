using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstApi.Data;
using MyFirstApi.Dto.Categories;

namespace MyFirstApi.Controllers
{
    public class CategoryController : ApiController
    {
        private readonly MyFirstApiDbContext _context;

        public CategoryController(MyFirstApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public Task<List<CategoryDto>> GetCategories() =>
            _context.Categories
                .Select(s => new CategoryDto
                {
                    CategoryID = s.CategoryId,
                    Name = s.Name
                })
                .ToListAsync();
    }
}