using Application.Exceptions;
using Application.Features.Comments.Common;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Comments.GetComments
{
    public class GetPostCommentsQueryHandler : IRequestHandler<GetPostCommentsQuery, GetPostCommentsResponse>
    {
        private readonly IApplicationDbContext _context;

        public GetPostCommentsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetPostCommentsResponse> Handle(GetPostCommentsQuery request, CancellationToken cancellationToken)
        {
            bool isModerator = request.AuthUserRoles.Contains("Moderator");

            if (request.AuthUserId == 0) throw new UnauthorizedException("You must be logged in to view comments");

            var comments = await _context.Comments
            .Where(comment => comment.PostId == request.PostId)
            .OrderByDescending(comment => comment.TimeCommented)
            .Select(comment => new CommentDto
            {
                CommentId = comment.CommentId,
                PostId = comment.PostId,
                AppUserId = comment.AppUserId,
                Username = comment.AppUser.UserName,
                FullName = comment.AppUser.FullName,
                Gender = comment.AppUser.Gender,
                ProfilePictureUrl = comment.AppUser.ProfilePictureUrl,
                Content = comment.Content,
                LikesCount = comment.LikesCount,
                TimeCommented = comment.TimeCommented,
                IsLikedByCurrentUser = comment.Likes.Any(like => like.AppUserId == request.AuthUserId),
                IsAuthorized = comment.AppUserId == request.AuthUserId || isModerator
            })
            .ToListAsync(cancellationToken);

            if (comments.Count == 0) throw new NotFoundException("There are no comments");

            return new GetPostCommentsResponse { Comments = comments };
        }
    }
}
