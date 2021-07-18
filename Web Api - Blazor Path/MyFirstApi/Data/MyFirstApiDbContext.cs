using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyFirstApi.Models;


namespace MyFirstApi.Data
{
    public class MyFirstApiDbContext : IdentityDbContext<User, Role, string>
    {
        public MyFirstApiDbContext(DbContextOptions<MyFirstApiDbContext> options)
            : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

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

            modelBuilder.Entity<Category>(config =>
            {
                config.HasKey(q => q.CategoryId);

                config.Property(q => q.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                config.HasMany(q => q.Products)
                    .WithOne(q => q.Category)
                    .HasForeignKey(q => q.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}