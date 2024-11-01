using Microsoft.EntityFrameworkCore;
using Social.Application.Interfaces;
using Social.Domain.Entities;
using Social.Infrastructure.Data.Configurations;

namespace Social.Infrastructure.Data
{
    public class DataContext : DbContext, ISocialDbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set;}
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<PrivateMessage> PrivateMessages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Connection> Connections { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // User
            builder.ApplyConfiguration(new UserConfiguration());

            // Posts
            builder.ApplyConfiguration(new PostConfiguration());

            // Comments
            builder.ApplyConfiguration(new CommentConfiguration());

            // Likes
            builder.ApplyConfiguration(new LikeConfiguration());

            // FriendRequest
            builder.ApplyConfiguration(new FriendRequestConfiguration());

            // Friendship
            builder.ApplyConfiguration(new FriendshipConfiguration());

            // PrivateMessages
            builder.ApplyConfiguration(new PrivateMessageConfiguration());
        }
    }
}