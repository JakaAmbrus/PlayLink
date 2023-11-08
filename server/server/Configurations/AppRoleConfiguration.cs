using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Entities;

namespace WebAPI.Configurations
{
    public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> entity)
        {
            entity.HasMany(ur => ur.UserRoles)
                 .WithOne(u => u.Role)
                 .HasForeignKey(ur => ur.RoleId)
                 .IsRequired();
        }
    }
}
