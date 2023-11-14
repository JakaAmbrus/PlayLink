using Application.Exceptions;
using Application.Features.Posts.Common;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Posts.GetPostsByUser
{
    public class GetPostsByUserQueryHandler : IRequestHandler<GetPostsByUserQuery, IEnumerable<PostDto>>
    {
        private readonly DataContext _context;

        public GetPostsByUserQueryHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PostDto>> Handle(GetPostsByUserQuery request, CancellationToken cancellationToken)
        {
            var requestedUser = await _context.Users.FindAsync(request.AppUserId, cancellationToken) 
                ?? throw new NotFoundException($"User with ID {request.AppUserId} not found");

            var posts = await _context.Posts
                .Where(p => p.AppUserId == request.AppUserId)
                .Select(p => new PostDto
                {
                    AppUserId = p.AppUserId,
                    PostId = p.PostId,
                    Description = p.Description,
                    DatePosted = p.DatePosted,
                    PhotoUrl = p.PhotoUrl,
                })
                .ToListAsync(cancellationToken);

            if(posts.Count == 0) throw new NotFoundException("User has no posts");

            return posts;
        }
    }
}
