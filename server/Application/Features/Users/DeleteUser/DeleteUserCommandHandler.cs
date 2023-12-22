using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, DeleteUserResponse>
    {
        private readonly IApplicationDbContext _context;

        public DeleteUserCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteUserResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.AuthUserId, cancellationToken)
                ?? throw new NotFoundException("Authorized user not found.");

            bool isGuestUser = await _context.Users.
                AnyAsync(u => u.Id == request.AuthUserId && u.UserRoles.Any(r => r.Role.Name == "Guest"), cancellationToken)
                && request.AuthUserRoles.Contains("Guest");

            if (isGuestUser) 
            { 
                throw new UnauthorizedException("Cannot delete a Guest User Account.");
            }

            bool isAdmin = await _context.Users.
                AnyAsync(u => u.Id == request.AuthUserId && u.UserRoles.Any(r => r.Role.Name == "Admin"), cancellationToken)
                && request.AuthUserRoles.Contains("Admin");

            if (isAdmin) 
            {
                throw new UnauthorizedException("An Administrator account cannot be deleted.");
            }

            using (var transaction = await _context.BeginTransactionAsync(cancellationToken))
            {
                try
                {

                    var userConnections = _context.Connections.Where(c => c.Username == user.UserName).ToList();
                    _context.Connections.RemoveRange(userConnections);

                    var posts = _context.Posts.Where(p => p.AppUserId == request.AuthUserId).ToList();
                    _context.Posts.RemoveRange(posts);

                    var sentRequests = _context.FriendRequests.Where(fr => fr.SenderId == request.AuthUserId).ToList();
                    var receivedRequests = _context.FriendRequests.Where(fr => fr.ReceiverId == request.AuthUserId).ToList();
                    _context.FriendRequests.RemoveRange(sentRequests.Concat(receivedRequests));

                    var friendshipsAsUser1 = _context.Friendships.Where(f => f.User1Id == request.AuthUserId).ToList();
                    var friendshipsAsUser2 = _context.Friendships.Where(f => f.User2Id == request.AuthUserId).ToList();
                    _context.Friendships.RemoveRange(friendshipsAsUser1.Concat(friendshipsAsUser2));

                    var sentMessages = _context.PrivateMessages.Where(msg => msg.SenderId == request.AuthUserId).ToList();
                    _context.PrivateMessages.RemoveRange(sentMessages);

                    var receivedMessages = _context.PrivateMessages.Where(msg => msg.RecipientId == request.AuthUserId).ToList();
                    _context.PrivateMessages.RemoveRange(receivedMessages);

                    _context.Users.Remove(user);

                    await _context.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);

                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw new ServerErrorException("Trouble deleting User.");
                }
            }

            return new DeleteUserResponse { IsDeleted = true };
        }
    }
}
