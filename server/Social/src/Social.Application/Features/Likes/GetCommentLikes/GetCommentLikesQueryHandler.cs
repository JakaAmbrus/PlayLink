using Social.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Features.Likes.Common;
using Social.Application.Interfaces;

namespace Social.Application.Features.Likes.GetCommentLikes
{
    public class GetCommentLikesQueryHandler : IRequestHandler<GetCommentLikesQuery, GetCommentLikesResponse>
    {
        private readonly IApplicationDbContext _context;

        public GetCommentLikesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetCommentLikesResponse> Handle(GetCommentLikesQuery request, CancellationToken cancellationToken)
        {
            var comment = await _context.Comments.FindAsync(new object[] { request.CommentId }, cancellationToken)
                ?? throw new NotFoundException("Comment not found");

            var likes = await _context.Likes
                .AsNoTracking()
                .Where(l => l.CommentId == request.CommentId && l.AppUserId != request.AuthUserId)
                .Include(l => l.AppUser)
                .ToListAsync(cancellationToken);

            var likedUsers = likes
                .Select(l => new LikedUserDto
                {
                    Username = l.AppUser.UserName,
                    FullName = l.AppUser.FullName,
                })
                .ToList();

            return new GetCommentLikesResponse { LikedUsers = likedUsers };
        }
    }
}
