using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configurations
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
