using ECommerceAPI.Modules.Orders.Models;
using ECommerceAPI.Modules.Orders.Validations;
using ECommerceAPI.Modules.Payments.Models;
using ECommerceAPI.Modules.Payments.Validations;
using ECommerceAPI.Modules.Products.Models;
using ECommerceAPI.Modules.Products.Validation;
using ECommerceAPI.Modules.ShoppingCarts.Models;
using ECommerceAPI.Modules.ShoppingCarts.Validations;
using ECommerceAPI.Modules.Products.Validation;
using ECommerceAPI.Modules.Users.Models;
using ECommerceAPI.Modules.Users.Validations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Shared.Database
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            

        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new ProductConfiguration());

            builder.ApplyConfiguration(new CartItemConfiguration());
            builder.ApplyConfiguration(new OrderConfiguration());
            builder.ApplyConfiguration(new OrderItemConfiguration());

            builder.ApplyConfiguration(new PaymentConfiguration());


        }
    }
}
