using Application.Exceptions;
using Application.Features.Posts.Common;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Posts.GetPostById
{
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, PostDto>
    {
        private readonly DataContext _context;

        public GetPostByIdQueryHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<PostDto> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
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

            return post ?? throw new NotFoundException($"Post with ID {request.PostId} not found.");
        }
    }
}
