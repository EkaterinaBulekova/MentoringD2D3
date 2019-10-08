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

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Cart> Carts { get; set; }

    }
}
