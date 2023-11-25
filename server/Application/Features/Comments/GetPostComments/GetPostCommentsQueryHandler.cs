using Application.Exceptions;
using Application.Features.Comments.Common;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Comments.GetComments
{
    public class GetPostCommentsQueryHandler : IRequestHandler<GetPostCommentsQuery, GetPostCommentsResponse>
    {
        private readonly DataContext _context;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public GetPostCommentsQueryHandler(DataContext context, IAuthenticatedUserService authenticatedUserService)
        {
            _context = context;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<GetPostCommentsResponse> Handle(GetPostCommentsQuery request, CancellationToken cancellationToken)
        {
            int currentUserId = _authenticatedUserService.UserId;
            var CurrentUserRole = _authenticatedUserService.UserRoles;

            bool isModerator = CurrentUserRole.Contains("Moderator");

            if (currentUserId == 0) throw new UnauthorizedException("You must be logged in to view comments");

            var comments = await _context.Comments
            .Where(comment => comment.PostId == request.PostId)
            .OrderByDescending(comment => comment.TimeCommented)
            .Select(comment => new CommentDto
            {
                CommentId = comment.CommentId,
                AppUserId = comment.AppUserId,
                Username = comment.AppUser.UserName,
                FullName = comment.AppUser.FullName,
                Gender = comment.AppUser.Gender,
                ProfilePictureUrl = comment.AppUser.ProfilePictureUrl,
                Content = comment.Content,
                TimeCommented = comment.TimeCommented,
                IsLikedByCurrentUser = comment.Likes.Any(like => like.AppUserId == currentUserId),
                IsAuthorized = comment.AppUserId == currentUserId || isModerator
            })
            .ToListAsync(cancellationToken);

            if (comments.Count == 0) throw new NotFoundException("There are no comments");

            return new GetPostCommentsResponse { Comments = comments };
        }
    }
}
