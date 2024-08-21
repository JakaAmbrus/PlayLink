using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> entity)
        {
            entity.HasOne(c => c.AppUser)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
