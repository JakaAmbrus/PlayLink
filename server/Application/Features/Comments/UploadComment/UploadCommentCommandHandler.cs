using Application.Exceptions;
using Application.Features.Comments.Common;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Features.Comments.UploadComment
{
    public class UploadCommentCommandHandler : IRequestHandler<UploadCommentCommand, UploadCommentResponse>
    {
        private readonly DataContext _context;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public UploadCommentCommandHandler(DataContext context,
            IAuthenticatedUserService authenticatedUserService)
        {
            _context = context;
            _authenticatedUserService = authenticatedUserService;
        }
        public async Task<UploadCommentResponse> Handle(UploadCommentCommand request, CancellationToken cancellationToken)
        {
            var post = await _context.Posts.FindAsync(request.CommentContent.PostId) 
                ?? throw new NotFoundException("Post not found");

            int currentUserId = _authenticatedUserService.UserId;
            var currentUser = await _context.Users.FindAsync(currentUserId);
            if (currentUser == null)
            {
                throw new NotFoundException("User not found");
            }

            var newComment = new Comment
            {
                AppUserId = currentUserId,
                PostId = request.CommentContent.PostId,
                Content = request.CommentContent.Content
            };

            post.CommentsCount++;
            _context.Comments.Add(newComment);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                throw new ServerErrorException("Problem saving comment");
            }

            return new UploadCommentResponse
            {
                CommentDto = new CommentDto
                {
                    AppUserId = newComment.AppUserId,
                    Username = currentUser.UserName,
                    FullName = currentUser.FullName,
                    TimeCommented = newComment.TimeCommented,
                    PostId = newComment.PostId,
                    Content = newComment.Content
                }
            };
            
        }
    }
}
