using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Data.Entities;

namespace Order.Data.EntityConfigurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payment");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.OrderId)
            .IsRequired();

        builder.Property(p => p.PaymentDate)
            .IsRequired();

        builder.Property(p => p.AmountPaid)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.PaymentMethod)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.PaymentStatus)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.StripePaymentIntentId)
            .HasMaxLength(100);

        builder.Property(p => p.StripeChargeId)
            .HasMaxLength(100);

        builder.HasOne(p => p.Order)
            .WithOne(o => o.Payment)
            .HasForeignKey<Payment>(p => p.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}