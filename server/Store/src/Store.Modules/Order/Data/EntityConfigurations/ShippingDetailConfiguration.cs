using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Data.Entities;

namespace Order.Data.EntityConfigurations;

public class ShippingDetailConfiguration : IEntityTypeConfiguration<ShippingDetail>
{
    public void Configure(EntityTypeBuilder<ShippingDetail> builder)
    {
        builder.ToTable("ShippingDetail");

        builder.HasKey(sd => sd.Id);

        builder.Property(sd => sd.OrderId)
            .IsRequired();

        builder.Property(sd => sd.ShippingAddress)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(sd => sd.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(sd => sd.State)
            .HasMaxLength(100);

        builder.Property(sd => sd.PostalCode)
            .HasMaxLength(20);

        builder.Property(sd => sd.Country)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(sd => sd.ShippingDate);

        builder.Property(sd => sd.DeliveryDate);

        builder.HasOne(sd => sd.Order)
            .WithOne(o => o.ShippingDetail)
            .HasForeignKey<ShippingDetail>(sd => sd.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}