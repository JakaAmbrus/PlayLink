using MediatR;
using Social.Application.Features.Comments.Common;
using Social.Application.Features.Comments.GetComments;
using Social.Application.Interfaces;
using Social.Application.Utils;
using Social.Domain.Enums;
using Social.Domain.Exceptions;

namespace Social.Application.Features.Comments.GetPostComments
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
            var post = await _context.Posts.FindAsync(new object[] { request.PostId }, cancellationToken)
                ?? throw new NotFoundException("Post not found");

            bool isModerator = request.AuthUserRoles.Contains(Role.Moderator.ToString());

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
