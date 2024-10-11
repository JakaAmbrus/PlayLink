using Social.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Features.Posts.Common;
using Social.Application.Interfaces;
using Social.Domain.Enums;

namespace Social.Application.Features.Posts.GetPostById
{
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, GetPostByIdResponse>
    {
        private readonly IApplicationDbContext _context;

        public GetPostByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetPostByIdResponse> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            bool isModerator = request.AuthUserRoles.Contains(Role.Moderator.ToString());

            var post = await _context.Posts
               .Where(p => p.PostId == request.PostId)
               .Select(p => new PostDto
               {
                   AppUserId = p.AppUserId,
                   PostId = p.PostId,
                   Description = p.Description,
                   DatePosted = p.DatePosted,
                   Username = p.AppUser.UserName,
                   FullName = p.AppUser.FullName,
                   PhotoUrl = p.PhotoUrl,
                   Gender = p.AppUser.Gender,
                   LikesCount = p.LikesCount,
                   CommentsCount = p.CommentsCount,
                   IsLikedByCurrentUser = p.Likes.Any(l => l.AppUserId == request.AuthUserId),
                   IsAuthorized = p.AppUserId == request.AuthUserId || isModerator
               })
               .FirstOrDefaultAsync(cancellationToken)
               ?? throw new NotFoundException("Post not found");

            return  new GetPostByIdResponse { Post = post };
        }
    }
}
