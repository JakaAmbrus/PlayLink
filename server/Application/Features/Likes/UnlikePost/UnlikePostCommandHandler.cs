using Application.Exceptions;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Likes.UnlikePost
{
    public class UnlikePostCommandHandler : IRequestHandler<UnlikePostCommand, UnlikePostResponse>
    {
        private readonly DataContext _context;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public UnlikePostCommandHandler(DataContext context, IAuthenticatedUserService authenticatedUserService)
        {
            _context = context;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<UnlikePostResponse> Handle(UnlikePostCommand request, CancellationToken cancellationToken)
        {
            var CurrentuserId = _authenticatedUserService.UserId;
            var like = await _context.Likes.FirstOrDefaultAsync(l => l.PostId == request.PostId && l.AppUserId == CurrentuserId, cancellationToken);
            var post = await _context.Posts.FindAsync(request.PostId, cancellationToken);

            if (like == null)
            {
                throw new NotFoundException("Like not found");
            }

            post.LikesCount--;
            _context.Likes.Remove(like);
            await _context.SaveChangesAsync(cancellationToken);

            return new UnlikePostResponse { Unliked = true };
        }
    }
}
