using server.Entities;
using Microsoft.EntityFrameworkCore;

namespace server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }
        public DbSet<AppUser> Users { get; set; }

    }
}
