using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<AppUser> Users { get; set; }
        DbSet<Post> Posts { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<Like> Likes { get; set; }
        DbSet<Friendship> Friendships { get; set; }
        DbSet<FriendRequest> FriendRequests { get; set; }
        DbSet<PrivateMessage> PrivateMessages { get; set; }
        DbSet<Group> Groups { get; set; }
        DbSet<Connection> Connections { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        void Add<TEntity>(TEntity entity) where TEntity : class;
    }
}