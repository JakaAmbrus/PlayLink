using Application.Interfaces;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Likes.UnlikePost
{
    public class UnlikePostCommandHandler : IRequestHandler<UnlikePostCommand, UnlikePostResponse>
    {
        private readonly IApplicationDbContext _context;

        public UnlikePostCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UnlikePostResponse> Handle(UnlikePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _context.Posts.FindAsync(new object[] { request.PostId }, cancellationToken)
                ?? throw new NotFoundException("Post not found");

            var like = await _context.Likes.FirstOrDefaultAsync(l => l.PostId == request.PostId 
                && l.AppUserId == request.AuthUserId, cancellationToken) 
                ?? throw new NotFoundException("Posts like not found");

            post.LikesCount--;
            _context.Likes.Remove(like);
            await _context.SaveChangesAsync(cancellationToken);

            return new UnlikePostResponse { Unliked = true };
        }
    }
}
