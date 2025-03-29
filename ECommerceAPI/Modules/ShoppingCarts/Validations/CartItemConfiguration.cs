using ECommerceAPI.Modules.ShoppingCarts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceAPI.Modules.ShoppingCarts.Validations
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.Property(c => c.Price)
                .HasColumnType("decimal(10,2)");
        }
    }
}
