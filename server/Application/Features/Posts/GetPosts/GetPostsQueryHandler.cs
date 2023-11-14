using Application.Exceptions;
using Application.Features.Posts.Common;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Posts.GetPosts
{
    public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, List<PostDto>>
    {
        private readonly DataContext _context;
        public GetPostsQueryHandler( DataContext context) 
        {
            _context = context;
        }
        public async Task<List<PostDto>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _context.Posts
            .Select(post => new PostDto
            {
                PostId = post.PostId,
                AppUserId = post.AppUserId,
                Description = post.Description,
                DatePosted = post.DatePosted,
                PhotoUrl = post.PhotoUrl,
            })
            .ToListAsync(cancellationToken);

            if (posts.Count == 0) throw new NotFoundException("There are no posts");
            
            return posts;
        }
    }
}
