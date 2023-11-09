using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configurations
{
    public class GroupChatUserConfiguration : IEntityTypeConfiguration<GroupChatUser>
    {
        public void Configure(EntityTypeBuilder<GroupChatUser> entity)
        {
            entity.HasKey(gcu => new { gcu.AppUserId, gcu.GroupChatId });

            entity.HasOne(gcu => gcu.AppUser)
                .WithMany(u => u.GroupChatUsers)
                .HasForeignKey(gcu => gcu.AppUserId);

            entity.HasOne(gcu => gcu.GroupChat)
                .WithMany(gc => gc.GroupChatUsers)
                .HasForeignKey(gcu => gcu.GroupChatId);
        }
    }
}
