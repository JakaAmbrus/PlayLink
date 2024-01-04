using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Likes.UnlikeComment
{
    public class UnlikeCommentCommandHandler : IRequestHandler<UnlikeCommentCommand, UnlikeCommentResponse>
    {
        private readonly IApplicationDbContext _context;

        public UnlikeCommentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UnlikeCommentResponse> Handle(UnlikeCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _context.Comments.FindAsync(request.CommentId, cancellationToken)
                ?? throw new NotFoundException("Comment not found");

            var like = await _context.Likes.FirstOrDefaultAsync(l => l.CommentId == request.CommentId 
                && l.AppUserId == request.AuthUserId, cancellationToken) 
                ?? throw new NotFoundException("Comments like not found");

            comment.LikesCount--;
            _context.Likes.Remove(like);
            await _context.SaveChangesAsync(cancellationToken);

            return new UnlikeCommentResponse { Unliked = true };
        } 
    }
}
