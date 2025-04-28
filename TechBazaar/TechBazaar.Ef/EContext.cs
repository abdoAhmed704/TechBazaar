using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechBazaar.Core.Models;

namespace TechBazaar.Ef
{
    public class EContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<WishItem> WishItems { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public EContext(DbContextOptions<EContext> options):base(options)
        {
           
        }
    }
}
