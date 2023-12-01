using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Likes.UnlikeComment
{
    public class UnlikeCommentCommandHandler : IRequestHandler<UnlikeCommentCommand, UnlikeCommentResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public UnlikeCommentCommandHandler(IApplicationDbContext context, IAuthenticatedUserService authenticatedUserService)
        {
            _context = context;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<UnlikeCommentResponse> Handle(UnlikeCommentCommand request, CancellationToken cancellationToken)
        {
            var CurrentuserId = _authenticatedUserService.UserId;

            var like = await _context.Likes.FirstOrDefaultAsync(l => l.CommentId == request.CommentId && l.AppUserId == CurrentuserId, cancellationToken);
            if (like == null)
            {
                throw new NotFoundException("Like not found");
            }

            var comment = await _context.Comments.FindAsync(request.CommentId, cancellationToken);
            if (comment == null)
            {
                   throw new NotFoundException("Comment not found");
            }

            comment.LikesCount--;
            _context.Likes.Remove(like);
            await _context.SaveChangesAsync(cancellationToken);

            return new UnlikeCommentResponse { Unliked = true };
        } 
    }
}
