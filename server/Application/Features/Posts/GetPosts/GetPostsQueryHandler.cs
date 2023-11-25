using Application.Exceptions;
using Application.Features.Posts.Common;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Posts.GetPosts
{
    public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, GetPostsResponse>
    {
        private readonly DataContext _context;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        public GetPostsQueryHandler( DataContext context, IAuthenticatedUserService authenticatedUserService) 
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

            var posts = await _context.Posts
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
            })
            .ToListAsync(cancellationToken);

            if (posts.Count == 0) throw new NotFoundException("There are no posts");
            
            return new GetPostsResponse { Posts = posts};
        }
    }
}
