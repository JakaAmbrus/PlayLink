﻿using Application.Exceptions;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Comments.DeleteComment
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, DeleteCommentResponse>
    {
        private readonly IApplicationDbContext  _context;

        public DeleteCommentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteCommentResponse> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var selectedComment = await _context.Comments.FindAsync(request.CommentId, cancellationToken)
                ?? throw new NotFoundException("Comment was not found");

            var selectedPost = await _context.Posts.FindAsync(selectedComment.PostId, cancellationToken)
                ?? throw new NotFoundException("Post was not found");

            bool isPostOwner = selectedComment.AppUserId == request.AuthUserId;
            bool isModerator = request.AuthUserRoles.Contains("Moderator");

            //Only the comments owner or a moderator/admin can delete a comment
            if (!isPostOwner && !isModerator)
            {
                throw new UnauthorizedException("User not authorized to delete comment");
            }

            selectedPost.CommentsCount--;
            _context.Comments.Remove(selectedComment);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                throw new ServerErrorException("Problem deleting Commet");
            }

            return new DeleteCommentResponse { IsDeleted = true };
        }
    }
}
