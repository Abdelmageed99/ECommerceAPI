using ECommerceAPI.Modules.Payments.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceAPI.Modules.Payments.Validations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(p => p.Amount)
                .HasColumnType("decimal(10,2)");

            builder.Property(p => p.PaymentMethod)
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(p => p.PaymentStatus)
               .IsRequired()
               .HasMaxLength(15);

            builder.Property(p => p.TransactionId)
               .IsRequired()
               .HasMaxLength(100);





        }
    }
}
