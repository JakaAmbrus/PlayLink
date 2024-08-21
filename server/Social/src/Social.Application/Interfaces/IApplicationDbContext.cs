using Social.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Social.Application.Interfaces
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

        void Add<TEntity>(TEntity entity) where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    }
}