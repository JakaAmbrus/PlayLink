using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Entities;

namespace server.Configurations
{
    public class PrivateMessageConfiguration : IEntityTypeConfiguration<PrivateMessage>
    {
        public void Configure(EntityTypeBuilder<PrivateMessage> entity)
        {
            entity.HasOne(pm => pm.Sender)
                .WithMany(user => user.MessagesSent)
                .HasForeignKey(msg => msg.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(pm => pm.Recipient)
                .WithMany(user => user.MessagesReceived)
                .HasForeignKey(msg => msg.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
