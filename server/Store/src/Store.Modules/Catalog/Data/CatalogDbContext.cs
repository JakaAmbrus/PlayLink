using Catalog.Data.Entities;
using Catalog.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Data;

public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    
    public DbSet<Category> Categories { get; set; }
    
    public DbSet<Brand> Brands { get; set; }
    
    public DbSet<ProductColor> ProductColors { get; set; }
    
    public DbSet<ProductImage> ProductImages { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Products
        builder.ApplyConfiguration(new ProductConfiguration());

        // Categories
        builder.ApplyConfiguration(new CategoryConfiguration());

        // Brands
        builder.ApplyConfiguration(new BrandConfiguration());

        // ProductColors
        builder.ApplyConfiguration(new ProductColorConfiguration());

        // ProductImages
        builder.ApplyConfiguration(new ProductImageConfiguration());
        
        builder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
    }
}