using Social.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Features.Likes.Common;
using Social.Application.Interfaces;

namespace Social.Application.Features.Likes.GetPostLikes
{
    public class GetPostLikesQueryHandler : IRequestHandler<GetPostLikesQuery, GetPostLikesResponse>
    {
        private readonly IApplicationDbContext _context;

        public GetPostLikesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetPostLikesResponse> Handle(GetPostLikesQuery request, CancellationToken cancellationToken)
        {
            var post = await _context.Posts.FindAsync(new object[] { request.PostId }, cancellationToken)
                ?? throw new NotFoundException("Post not found");

            var likes = await _context.Likes
                .AsNoTracking()
                .Where(l => l.PostId == request.PostId && l.AppUserId != request.AuthUserId)
                .Include(l => l.AppUser)
                .ToListAsync(cancellationToken);

            var likedUsers = likes
                .Select(l => new LikedUserDto
                {
                    Username = l.AppUser.UserName,
                    FullName = l.AppUser.FullName,
                })
                .ToList();

            return new GetPostLikesResponse { LikedUsers = likedUsers };
        }
    }
}
