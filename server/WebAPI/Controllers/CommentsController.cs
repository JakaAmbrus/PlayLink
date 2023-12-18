using Application.Features.Comments.Common;
using Application.Features.Comments.DeleteComment;
using Application.Features.Comments.GetComments;
using Application.Features.Comments.UploadComment;
using Application.Features.Likes.GetCommentLikes;
using Application.Features.Likes.LikeComment;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Manages comments related operations.
    /// </summary>
    public class CommentsController : BaseAuthApiController
    {
        public CommentsController(ISender mediator, IAuthenticatedUserService authenticatedUserService) : base(mediator, authenticatedUserService)
        {
        }

        /// <summary>
        /// Gets all comments for a post.
        /// </summary>
        /// <param name="postId">Post ID.</param>
        /// <returns>A list of all comments from a post.</returns>
        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostComments(int postId, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();
            IEnumerable<string> authUserRoles = GetCurrentUserRoles();

            var query = new GetPostCommentsQuery { 
                PostId = postId,
                AuthUserId = authUserId,
                AuthUserRoles = authUserRoles 
            };

            var result = await Mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Uploads a comment to a post.
        /// </summary>
        /// <param name="postId">Post ID.</param>
        /// <param name="comment">Text content of a comment.</param>
        /// <returns>Comment DTO of the uploaded comment.</returns>
        [HttpPost]
        public async Task<IActionResult> UploadComment(CommentUploadDto commentUploadDto, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();

            var command = new UploadCommentCommand
            {
                Comment = commentUploadDto,
                AuthUserId = authUserId, 
            };

            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Deletes a comment.
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Confirmation of deletion.</returns>
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();
            IEnumerable<string> authUserRoles = GetCurrentUserRoles();

            var command = new DeleteCommentCommand
            {
                CommentId = commentId,
                AuthUserId = authUserId,
                AuthUserRoles = authUserRoles
            };

            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Returns a list of users who liked a comment.
        /// </summary>
        /// <param name="commentId">Comment ID.</param>
        /// <returns>List of user DTOs.</returns>
        [HttpGet("{commentId}/likes")]
        public async Task<IActionResult> GetCommentLikes(int commentId, CancellationToken cancellationToken)
        {
            var query = new GetCommentLikesQuery { CommentId = commentId };

            var result = await Mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Adds a like to a comment.
        /// </summary>
        /// <param name="commentId">Comment ID.</param>
        /// <returns>Like confirmation.</returns>
        [HttpPost("{commentId}/like")]
        public async Task<IActionResult> LikeComment(int commentId, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();

            var command = new LikeCommentCommand { 
                CommentId = commentId,
                AuthUserId = authUserId
            };

            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Removes a like from a comment.
        /// </summary>
        /// <param name="commentId">Comment ID.</param>
        /// <returns>Confirmation of the unlike.</returns>
        [HttpDelete("{commentId}/like")]
        public async Task<IActionResult> UnlikeComment(int commentId, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();

            var command = new LikeCommentCommand
            {
                CommentId = commentId,
                AuthUserId = authUserId
            };

            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }
    }
}
