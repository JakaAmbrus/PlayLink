using Discount.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Discount.Data.EntityConfigurations;

public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.ToTable("Coupon");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.IsActive)
            .IsRequired();

        builder.Property(c => c.UsageLimit)
            .IsRequired(false);

        builder.Property(c => c.UsageCount)
            .IsRequired();

        builder.Property(c => c.ExpirationDate)
            .IsRequired();

        builder.HasOne(c => c.Discount)
            .WithMany(d => d.Coupons)
            .HasForeignKey(c => c.DiscountId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(c => c.UserId)
            .IsRequired(false);
    }
}