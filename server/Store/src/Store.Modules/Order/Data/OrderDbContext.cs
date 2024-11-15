using Microsoft.EntityFrameworkCore;

namespace Order.Data;

public class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options)
{
    public DbSet<Entities.Order> Orders { get; set; }
    
    public DbSet<Entities.OrderItem> OrderItems { get; set; }
    
    public DbSet<Entities.Payment> Payments { get; set; }
    
    public DbSet<Entities.ShippingDetail> ShippingDetails { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderDbContext).Assembly);
    }
}