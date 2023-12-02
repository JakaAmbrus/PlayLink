using Application.Exceptions;
using Application.Features.Posts.Common;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Posts.GetPostById
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
            var post = await _context.Posts
               .Where(p => p.PostId == request.PostId)
               .Select(p => new PostDto
               {
                   AppUserId = p.AppUserId,
                   PostId = p.PostId,
                   Description = p.Description,
                   DatePosted = p.DatePosted,
                   PhotoUrl = p.PhotoUrl,        
               })
               .FirstOrDefaultAsync(cancellationToken);

            return post == null
                ? throw new NotFoundException($"Post with ID {request.PostId} not found.")
                : new GetPostByIdResponse { Post = post};
        }
    }
}
