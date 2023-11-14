using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class FriendshipConfiguration : IEntityTypeConfiguration<Friendship>
    {
        public void Configure(EntityTypeBuilder<Friendship> entity)
        {
            entity.HasKey(f => f.FriendshipId);

            entity.HasOne(f => f.User1)
                .WithMany(u => u.Friends)
                .HasForeignKey(f => f.User1Id)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(f => f.User2)
                .WithMany(u => u.Friends)
                .HasForeignKey(f => f.User2Id)
                .OnDelete(DeleteBehavior.Restrict);

            //Prevent duplicate friendships
            entity.HasIndex(f => new { f.User1Id, f.User2Id }).IsUnique();
        }
    }
}
