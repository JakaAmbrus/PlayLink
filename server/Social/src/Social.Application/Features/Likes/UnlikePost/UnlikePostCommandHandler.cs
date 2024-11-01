using Social.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Interfaces;

namespace Social.Application.Features.Likes.UnlikePost
{
    public class UnlikePostCommandHandler : IRequestHandler<UnlikePostCommand, UnlikePostResponse>
    {
        private readonly ISocialDbContext _context;

        public UnlikePostCommandHandler(ISocialDbContext context)
        {
            _context = context;
        }

        public async Task<UnlikePostResponse> Handle(UnlikePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _context.Posts.FindAsync(request.PostId)
                ?? throw new NotFoundException("Post not found");

            var like = await _context.Likes
                .Where(l => l.PostId == request.PostId && l.AppUserId == request.AuthUserId)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (like == null)
            {
                return new UnlikePostResponse { Unliked = true };
            }

            post.LikesCount--;
            _context.Likes.Remove(like);
            await _context.SaveChangesAsync(cancellationToken);

            return new UnlikePostResponse { Unliked = true };
        }
    }
}
