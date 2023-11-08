using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Entities;

namespace WebAPI.Configurations
{
    public class LikeConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> entity)
        {
            entity.HasOne(l => l.AppUser)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.AppUserId);

            entity.HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostId);

            entity.HasOne(l => l.Comment)
              .WithMany(c => c.Likes)
              .HasForeignKey(l => l.CommentId);
        }
    }
}
