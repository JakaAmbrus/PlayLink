using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> entity)
        {
            entity.HasMany(ur => ur.UserRoles)
                 .WithOne(u => u.User)
                 .HasForeignKey(ur => ur.UserId)
                 .IsRequired();
        }
    }
}
