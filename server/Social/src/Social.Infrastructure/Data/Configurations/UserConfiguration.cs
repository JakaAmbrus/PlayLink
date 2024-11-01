using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Social.Domain.Entities;

namespace Social.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
          entity.HasKey(u => u.Id);

            entity.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.HasIndex(u => u.Username)
                .IsUnique();

            entity.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.Gender)
                .IsRequired()
                .HasMaxLength(6);

            entity.Property(u => u.Country)
                .IsRequired()
                .HasMaxLength(60);

            entity.Property(u => u.DateOfBirth)
                .IsRequired();

            entity.Property(u => u.Description)
                .HasMaxLength(200);
        }
    }
}
