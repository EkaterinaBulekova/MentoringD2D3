using FoodShopDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodShopDAL
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Cart> Carts { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<Category>()
        //       .HasMany(e => e.Products)
        //       .WithOne(e => e.Category)
        //       .OnDelete(DeleteBehavior.Cascade);

        //    modelBuilder.Entity<Product>()
        //        .HasOne(e => e.Category)
        //        .WithMany(e => e.Products)
        //        .OnDelete(DeleteBehavior.ClientSetNull);

        //}

    }
}
