using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Likes.LikeComment
{
    public class LikeCommentCommandHandler : IRequestHandler<LikeCommentCommand, LikeCommentResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public LikeCommentCommandHandler(IApplicationDbContext context, IAuthenticatedUserService authenticatedUserService)
        {
            _context = context;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<LikeCommentResponse> Handle(LikeCommentCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _authenticatedUserService.UserId;

            var comment = await _context.Comments.FindAsync(request.CommentId);

            if (comment == null)
            {
                throw new NotFoundException("Comment not found");
            }

            var existingLike = await _context.Likes
                .AnyAsync(l => l.CommentId == request.CommentId && l.AppUserId == currentUserId);

            if (existingLike)
            {
                throw new BadRequestException("You have already liked this comment");
            }

            var like = new Like
            {
                AppUserId = currentUserId,
                CommentId = request.CommentId
            };

            comment.LikesCount++;
            _context.Likes.Add(like);

            await _context.SaveChangesAsync(cancellationToken);

            return new LikeCommentResponse { Liked = true };
        }   

    }
}
