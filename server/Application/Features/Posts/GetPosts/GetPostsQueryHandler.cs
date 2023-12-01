using Application.Exceptions;
using Application.Features.Posts.Common;
using Application.Interfaces;
using Application.Utils;
using MediatR;

namespace Application.Features.Posts.GetPosts
{
    public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, GetPostsResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        public GetPostsQueryHandler(IApplicationDbContext context, IAuthenticatedUserService authenticatedUserService) 
        {
            _context = context;
            _authenticatedUserService = authenticatedUserService;
        }
        public async Task<GetPostsResponse> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            int currentUserId = _authenticatedUserService.UserId;
            var CurrentUserRole = _authenticatedUserService.UserRoles;

            bool isModerator = CurrentUserRole.Contains("Moderator");

            if (currentUserId == 0) throw new UnauthorizedException("You must be logged in to view posts");

            var posts = _context.Posts
            .AsQueryable()
            .OrderByDescending(post => post.DatePosted)
            .Select(post => new PostDto
            {
                PostId = post.PostId,
                AppUserId = post.AppUserId,
                Username = post.AppUser.UserName,
                FullName = post.AppUser.FullName,
                ProfilePictureUrl = post.AppUser.ProfilePictureUrl,
                Gender = post.AppUser.Gender,
                Description = post.Description,
                DatePosted = post.DatePosted,
                PhotoUrl = post.PhotoUrl,
                LikesCount = post.LikesCount,
                CommentsCount = post.CommentsCount,
                IsLikedByCurrentUser = post.Likes.Any(like => like.AppUserId == currentUserId),
                IsAuthorized = post.AppUserId == currentUserId || isModerator
            });

            var pagedPosts = await PagedList<PostDto>
                .CreateAsync(posts, request.Params.PageNumber, request.Params.PageSize);

            return new GetPostsResponse { Posts = pagedPosts };
        }
    }
}
