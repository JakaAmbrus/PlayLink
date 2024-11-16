using Catalog.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Data.EntityConfigurations;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable("ProductImage");

        builder.HasKey(pi => pi.Id);

        builder.Property(pi => pi.ImageUrl)
            .IsRequired()
            .HasMaxLength(300);
        
        builder.Property(pi => pi.ImagePublicId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(pi => pi.DisplayOrder)
            .IsRequired();

        builder.Property(pi => pi.IsPrimary)
            .IsRequired();

        builder.HasOne(pi => pi.Product)
            .WithMany(p => p.Images)
            .HasForeignKey(pi => pi.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}