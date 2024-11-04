using MediatR;
using Shared.Core.Security;
using Social.Application.Interfaces;
using Social.Domain.Exceptions;

namespace Social.Application.Features.Comments.DeleteComment
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, DeleteCommentResponse>
    {
        private readonly ISocialDbContext  _context;

        public DeleteCommentCommandHandler(ISocialDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteCommentResponse> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var selectedComment = await _context.Comments.FindAsync(request.CommentId)
                ?? throw new NotFoundException("Comment was not found");

            var selectedPost = await _context.Posts.FindAsync(selectedComment.PostId)
                ?? throw new NotFoundException("Post was not found");

            bool isPostOwner = selectedComment.AppUserId == request.AuthUserId;
            bool isModerator = request.AuthUserRoles.Contains(Roles.Moderator);
            
            if (!isPostOwner && !isModerator)
            {
                throw new UnauthorizedException("User not authorized to delete comment");
            }

            selectedPost.CommentsCount--;
            _context.Comments.Remove(selectedComment);

            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteCommentResponse { IsDeleted = true };
        }
    }
}
