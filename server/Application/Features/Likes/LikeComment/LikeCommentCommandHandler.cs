using Application.Exceptions;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Likes.LikeComment
{
    public class LikeCommentCommandHandler : IRequestHandler<LikeCommentCommand, LikeCommentResponse>
    {
        private readonly DataContext _context;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public LikeCommentCommandHandler(DataContext context, IAuthenticatedUserService authenticatedUserService)
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
