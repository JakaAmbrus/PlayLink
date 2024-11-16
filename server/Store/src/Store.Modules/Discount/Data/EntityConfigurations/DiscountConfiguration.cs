using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Discount.Data.EntityConfigurations;

public class DiscountConfiguration : IEntityTypeConfiguration<Entities.Discount>
{
    public void Configure(EntityTypeBuilder<Entities.Discount> builder)
    {
        builder.ToTable("Discount");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Description)
            .HasMaxLength(500);

        builder.Property(d => d.DiscountPercentage)
            .HasColumnType("decimal(5,2)");

        builder.Property(d => d.FixedAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(d => d.IsPercentage)
            .IsRequired();

        builder.Property(d => d.ValidFrom)
            .IsRequired();

        builder.Property(d => d.ValidTo)
            .IsRequired();

        builder.Property(d => d.MinimumOrderAmount)
            .HasColumnType("decimal(18,2)");
    }
}