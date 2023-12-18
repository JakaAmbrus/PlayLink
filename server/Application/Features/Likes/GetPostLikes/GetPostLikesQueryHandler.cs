using Application.Exceptions;
using Application.Features.Likes.Common;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Likes.GetPostLikes
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
                .Where(l => l.PostId == request.PostId)
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
