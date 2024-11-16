using Catalog.Data.Entities;
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
        
        builder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
    }
}