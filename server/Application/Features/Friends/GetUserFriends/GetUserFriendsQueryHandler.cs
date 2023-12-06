using Application.Features.Friends.Common;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Friends.GetUserFriends
{
    public class GetUserFriendsQueryHandler : IRequestHandler<GetUserFriendsQuery, GetUserFriendsResponse>
    {
        private readonly IApplicationDbContext _context;

        public GetUserFriendsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetUserFriendsResponse> Handle(GetUserFriendsQuery request, CancellationToken cancellationToken)
        {
            var friends = await _context.Friendships
                .Where(f => f.User1Id == request.AuthUserId || f.User2Id == request.AuthUserId)
                .SelectMany(f => _context.Users
                .Where(u => u.Id == (f.User1Id == request.AuthUserId ? f.User2Id : f.User1Id))
                .Select(u => new FriendDto
                {
                    Username = u.UserName,
                    FullName = u.FullName,
                    ProfilePictureUrl = u.ProfilePictureUrl,
                    Gender = u.Gender,
                    DateEstablished = f.DateEstablished
                }))
                .Distinct()
                .ToListAsync(cancellationToken);

            return new GetUserFriendsResponse
            {
                Friends = friends
            };
        }
    }
}
