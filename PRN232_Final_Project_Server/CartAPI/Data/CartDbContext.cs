using CartAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CartAPI.Data
{
    public class CartDbContext : DbContext
    {
        public CartDbContext(DbContextOptions<CartDbContext> options)
            : base(options) { }

        public DbSet<Cart> Carts { get; set; }
    }
}

