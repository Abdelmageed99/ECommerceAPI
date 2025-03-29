using ECommerceAPI.Modules.Orders.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceAPI.Modules.Orders.Validations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.Status)
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(o => o.TotalPrice)
                .HasColumnType("decimal(10,2)");
        }
    }
}
