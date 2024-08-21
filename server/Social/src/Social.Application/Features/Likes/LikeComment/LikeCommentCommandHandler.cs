using Social.Domain.Entities;
using Social.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Interfaces;

namespace Social.Application.Features.Likes.LikeComment
{
    public class LikeCommentCommandHandler : IRequestHandler<LikeCommentCommand, LikeCommentResponse>
    {
        private readonly IApplicationDbContext _context;

        public LikeCommentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LikeCommentResponse> Handle(LikeCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _context.Comments.FindAsync(new object[] { request.CommentId }, cancellationToken) 
                ?? throw new NotFoundException("Comment not found");

            var existingLike = await _context.Likes
                .AnyAsync(l => l.CommentId == request.CommentId && l.AppUserId == request.AuthUserId, cancellationToken: cancellationToken);

            if (existingLike)
            {
                throw new BadRequestException("You have already liked this comment");
            }

            var like = new Like
            {
                AppUserId = request.AuthUserId,
                CommentId = request.CommentId
            };

            comment.LikesCount++;
            _context.Likes.Add(like);

            await _context.SaveChangesAsync(cancellationToken);

            return new LikeCommentResponse { Liked = true };
        }   

    }
}
