using Application.Exceptions;
using Application.Features.Likes.Common;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Likes.GetCommentLikes
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
                .Where(l => l.CommentId == request.CommentId)
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
