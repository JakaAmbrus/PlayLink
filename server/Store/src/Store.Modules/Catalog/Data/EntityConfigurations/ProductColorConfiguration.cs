using Catalog.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Data.EntityConfigurations;

public class ProductColorConfiguration : IEntityTypeConfiguration<ProductColor>
{
    public void Configure(EntityTypeBuilder<ProductColor> builder)
    {
        builder.ToTable("ProductColor");

        builder.HasKey(pc => pc.Id);

        builder.Property(pc => pc.ColorName)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(pc => pc.HexCode)
            .IsRequired()
            .HasMaxLength(7); // Example: #FFFFFF

        builder.Property(pc => pc.InventoryQuantity)
            .IsRequired();

        builder.HasOne(pc => pc.Product)
            .WithMany(p => p.Colors)
            .HasForeignKey(pc => pc.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}