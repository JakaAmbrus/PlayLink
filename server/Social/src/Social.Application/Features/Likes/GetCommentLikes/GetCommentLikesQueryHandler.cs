using Social.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Features.Likes.Common;
using Social.Application.Interfaces;

namespace Social.Application.Features.Likes.GetCommentLikes
{
    public class GetCommentLikesQueryHandler : IRequestHandler<GetCommentLikesQuery, GetCommentLikesResponse>
    {
        private readonly ISocialDbContext _context;

        public GetCommentLikesQueryHandler(ISocialDbContext context)
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
                .Include(l => l.User)
                .ToListAsync(cancellationToken);

            var likedUsers = likes
                .Select(l => new LikedUserDto
                {
                    Username = l.User.Username,
                    FullName = l.User.FullName,
                })
                .ToList();

            return new GetCommentLikesResponse { LikedUsers = likedUsers };
        }
    }
}
