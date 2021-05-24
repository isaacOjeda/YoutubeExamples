using Microsoft.EntityFrameworkCore;
using MyFirstApi.Models;

namespace MyFirstApi.Data
{
    public class MyFirstApiDbContext : DbContext
    {
        public MyFirstApiDbContext(DbContextOptions<MyFirstApiDbContext> options)
            : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(config =>
            {
                config.HasKey(q => q.ProductId);

                config.Property(q => q.Description)
                    .HasMaxLength(1000);

                config.Property(q => q.Name)
                    .HasMaxLength(500)
                    .IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}