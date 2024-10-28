using Microsoft.EntityFrameworkCore;

namespace Discount.Data;

public class DiscountDbContext : DbContext
{
    public DiscountDbContext(DbContextOptions<DiscountDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DiscountDbContext).Assembly);
    }
}