using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Entities;

namespace WebAPI.Configurations
{
    public class GroupMessageConfiguration : IEntityTypeConfiguration<GroupMessage>
    {
        public void Configure(EntityTypeBuilder<GroupMessage> entity)
        {
            entity.HasOne(gm => gm.GroupChat)
                .WithMany(gc => gc.GroupMessages)
                .HasForeignKey(gm => gm.GroupChatId);

            entity.HasOne(gm => gm.AppUser)
                .WithMany(u => u.GroupMessages)
                .HasForeignKey(gm => gm.AppUserId);
        }
    }
}
