using server.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace server.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
          IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
          IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();


            //Posts
            builder.Entity<Post>()
                .HasOne(p => p.AppUser)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.AppUserId);
          
            builder.Entity<Post>()
                .HasOne(p => p.Photo)
                .WithOne(c => c.Post)
                .HasForeignKey<Photo>(photo => photo.PostId);
       

            //Comments
            builder.Entity<Comment>()
                .HasOne(c => c.AppUser)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.AppUserId);
  

            builder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId);
     

            //Likes
            builder.Entity<Like>()
                .HasOne(l => l.AppUser)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.AppUserId);
         

            builder.Entity<Like>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostId);
         

            builder.Entity<Like>()
                .HasOne(l => l.Comment)
                .WithMany(c => c.Likes)
                .HasForeignKey(l => l.CommentId);
         

            //Notifications
            builder.Entity<Notification>()
                .HasOne(n => n.AppUser)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.AppUserId);
         

            builder.Entity<Notification>()
                .HasOne(n => n.Post)
                .WithMany(p => p.Notifications)
                .HasForeignKey(n => n.PostId);
         

            //FriendRequest
            builder.Entity<FriendRequest>()
                .HasOne(fr => fr.Sender)
                .WithMany(u => u.SentFriendRequests)
                .HasForeignKey(fr => fr.SenderId);
          

            builder.Entity<FriendRequest>()
                .HasOne(fr => fr.Receiver)
                .WithMany(u => u.ReceivedFriendRequests)
                .HasForeignKey(fr => fr.ReceiverId);
           

            //GroupChat
            builder.Entity<GroupChatUser>()
                .HasKey(gc => new { gc.AppUserId, gc.GroupChatId });

            builder.Entity<GroupChatUser>()
                .HasOne(gc => gc.AppUser)
                .WithMany(au => au.GroupChatUsers)
                .HasForeignKey(gc => gc.AppUserId);

            builder.Entity<GroupChatUser>()
                .HasOne(gc => gc.GroupChat)
                .WithMany(gc => gc.GroupChatUsers)
                .HasForeignKey(gc => gc.GroupChatId);

            builder.Entity<GroupMessage>()
                .HasOne(gm => gm.GroupChat)
                .WithMany(gc => gc.GroupMessages)
                .HasForeignKey(gm => gm.GroupChatId);

            builder.Entity<GroupMessage>()
                .HasOne(gm => gm.AppUser)
                .WithMany(au => au.GroupMessages)
                .HasForeignKey(gm => gm.AppUserId);
                

            //messages
            builder.Entity<Message>(entity =>
            {
                entity.HasOne(msg => msg.Sender)
                      .WithMany(user => user.MessagesSent)
                      .HasForeignKey(msg => msg.SenderId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(msg => msg.Recipient)
                      .WithMany(user => user.MessagesReceived)
                      .HasForeignKey(msg => msg.RecipientId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

    }
}
