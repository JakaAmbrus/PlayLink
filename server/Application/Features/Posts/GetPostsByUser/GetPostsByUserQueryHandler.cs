using Application.Exceptions;
using Application.Features.Posts.Common;
using Application.Interfaces;
using Application.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Posts.GetPostsByUser
{
    public class GetPostsByUserQueryHandler : IRequestHandler<GetPostsByUserQuery, GetPostsByUserResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public GetPostsByUserQueryHandler(IApplicationDbContext context, IAuthenticatedUserService authenticatedUserService)
        {
            _context = context;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<GetPostsByUserResponse> Handle(GetPostsByUserQuery request, CancellationToken cancellationToken)
        {
            int currentUserId = _authenticatedUserService.UserId;

            if (currentUserId == 0) throw new UnauthorizedException("You must be logged in to view posts");

            var CurrentUserRole = _authenticatedUserService.UserRoles;

            bool isModerator = CurrentUserRole.Contains("Moderator");

            var requestedUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.Username, cancellationToken)
                ?? throw new NotFoundException($"User with Username {request.Username} not found");

            var requestUserId = requestedUser.Id;

            var posts = _context.Posts
                .AsQueryable()
                .Where(p => p.AppUserId == requestUserId)
                .OrderByDescending(post => post.DatePosted)
                .Select(p => new PostDto
                {
                    AppUserId = p.AppUserId,
                    PostId = p.PostId,
                    Username = requestedUser.UserName,
                    FullName = requestedUser.FullName,
                    Gender = requestedUser.Gender,
                    Description = p.Description,
                    DatePosted = p.DatePosted,
                    PhotoUrl = p.PhotoUrl,
                    ProfilePictureUrl = requestedUser.ProfilePictureUrl,
                    LikesCount = p.LikesCount,
                    CommentsCount = p.CommentsCount,
                    IsLikedByCurrentUser = p.Likes.Any(like => like.AppUserId == currentUserId),
                    IsAuthorized = p.AppUserId == currentUserId || isModerator      
                });

            var pagedPosts = await PagedList<PostDto>
                .CreateAsync(posts, request.Params.PageNumber, request.Params.PageSize);

            return new GetPostsByUserResponse { Posts = pagedPosts };
        }
    }
}
