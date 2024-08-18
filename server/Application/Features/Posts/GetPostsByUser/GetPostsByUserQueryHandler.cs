using Application.Features.Posts.Common;
using Application.Interfaces;
using Application.Utils;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Posts.GetPostsByUser
{
    public class GetPostsByUserQueryHandler : IRequestHandler<GetPostsByUserQuery, GetPostsByUserResponse>
    {
        private readonly IApplicationDbContext _context;

        public GetPostsByUserQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetPostsByUserResponse> Handle(GetPostsByUserQuery request, CancellationToken cancellationToken)
        {
            bool isModerator = request.AuthUserRoles.Contains("Moderator");

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
                    IsLikedByCurrentUser = p.Likes.Any(like => like.AppUserId == request.AuthUserId),
                    IsAuthorized = p.AppUserId == request.AuthUserId || isModerator      
                });

            var pagedPosts = await PagedList<PostDto>
                .CreateAsync(posts, request.Params.PageNumber, request.Params.PageSize);

            return new GetPostsByUserResponse { Posts = pagedPosts };
        }
    }
}
