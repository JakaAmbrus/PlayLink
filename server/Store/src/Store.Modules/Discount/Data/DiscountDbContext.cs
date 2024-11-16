using Discount.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Discount.Data;

public class DiscountDbContext(DbContextOptions<DiscountDbContext> options) : DbContext(options)
{
    public DbSet<Coupon> Coupons { get; set; }
        
    public DbSet<Entities.Discount> Discounts { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DiscountDbContext).Assembly);
    }
}