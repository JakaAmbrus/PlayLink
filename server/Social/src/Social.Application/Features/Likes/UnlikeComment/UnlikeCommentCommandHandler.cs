using Social.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Interfaces;

namespace Social.Application.Features.Likes.UnlikeComment
{
    public class UnlikeCommentCommandHandler : IRequestHandler<UnlikeCommentCommand, UnlikeCommentResponse>
    {
        private readonly ISocialDbContext _context;

        public UnlikeCommentCommandHandler(ISocialDbContext context)
        {
            _context = context;
        }

        public async Task<UnlikeCommentResponse> Handle(UnlikeCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _context.Comments.FindAsync(request.CommentId)
                ?? throw new NotFoundException("Comment not found");

            var like = await _context.Likes
                .Where(l => l.CommentId == request.CommentId && l.AppUserId == request.AuthUserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (like == null)
            {
                return new UnlikeCommentResponse { Unliked = true };
            }
            
            comment.LikesCount--;
            _context.Likes.Remove(like);
            await _context.SaveChangesAsync(cancellationToken);

            return new UnlikeCommentResponse { Unliked = true };
        } 
    }
}
