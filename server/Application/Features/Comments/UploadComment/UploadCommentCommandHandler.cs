using Application.Exceptions;
using Application.Features.Comments.Common;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.Comments.UploadComment
{
    public class UploadCommentCommandHandler : IRequestHandler<UploadCommentCommand, UploadCommentResponse>
    {
        private readonly IApplicationDbContext _context;

        public UploadCommentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<UploadCommentResponse> Handle(UploadCommentCommand request, CancellationToken cancellationToken)
        {
            var post = await _context.Posts.FindAsync(new object[] { request.Comment.PostId }, cancellationToken)
                ?? throw new NotFoundException("Post not found");

            var authUser = await _context.Users.FindAsync(new object[] { request.AuthUserId }, cancellationToken) 
                ?? throw new NotFoundException("User not found");

            var newComment = new Comment
            {
                AppUserId = request.AuthUserId,
                PostId = request.Comment.PostId,
                Content = request.Comment.Content
            };

            post.CommentsCount++;
            _context.Comments.Add(newComment);

            await _context.SaveChangesAsync(cancellationToken);

            return new UploadCommentResponse
            {
                CommentDto = new CommentDto
                {
                    CommentId = newComment.CommentId,
                    PostId = newComment.PostId,
                    AppUserId = newComment.AppUserId,
                    Username = authUser.UserName,
                    FullName = authUser.FullName,
                    Gender = authUser.Gender,
                    ProfilePictureUrl = authUser.ProfilePictureUrl,
                    LikesCount = newComment.LikesCount,
                    IsAuthorized = true,
                    IsLikedByCurrentUser = false,
                    TimeCommented = newComment.TimeCommented,
                    Content = newComment.Content
                }
            };           
        }
    }
}
