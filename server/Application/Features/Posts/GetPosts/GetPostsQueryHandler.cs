using Application.Features.Posts.Common;
using Application.Interfaces;
using Application.Utils;
using MediatR;

namespace Application.Features.Posts.GetPosts
{
    public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, GetPostsResponse>
    {
        private readonly IApplicationDbContext _context;

        public GetPostsQueryHandler(IApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<GetPostsResponse> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            bool isModerator = request.AuthUserRoles.Contains("Moderator");

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
                IsLikedByCurrentUser = post.Likes.Any(like => like.AppUserId == request.AuthUserId),
                IsAuthorized = post.AppUserId == request.AuthUserId || isModerator
            });

            var pagedPosts = await PagedList<PostDto>
                .CreateAsync(posts, request.Params.PageNumber, request.Params.PageSize);

            return new GetPostsResponse { Posts = pagedPosts };
        }
    }
}
