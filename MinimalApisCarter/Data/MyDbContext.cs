using Microsoft.EntityFrameworkCore;
using MinimalApisCarter.Features.Products.Models;

namespace MinimalApisCarter.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options): base(options)
        {   
        }

        public DbSet<Product> Products => Set<Product>();
    }
}