using Application.Exceptions;
using Application.Features.Comments.Common;
using Application.Interfaces;
using Application.Utils;
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

            var comments = _context.Comments
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
            });

            var pagedComments = await PagedList<CommentDto>
                .CreateAsync(comments, request.Params.PageNumber, request.Params.PageSize);

            return new GetPostCommentsResponse { Comments = pagedComments };
        }
    }
}
