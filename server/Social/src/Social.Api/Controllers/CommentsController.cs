using Social.Application.Features.Comments.Common;
using Social.Application.Features.Comments.DeleteComment;
using Social.Application.Features.Comments.GetComments;
using Social.Application.Features.Comments.UploadComment;
using Social.Application.Features.Likes.GetCommentLikes;
using Social.Application.Features.Likes.LikeComment;
using Social.Application.Features.Likes.UnlikeComment;
using Social.Application.Utils;
using Microsoft.AspNetCore.Mvc;
using Social.Api.Extensions;

namespace Social.Api.Controllers
{
    /// <summary>
    /// Manages comments related operations.
    /// </summary>
    public class CommentsController : BaseController
    {
        /// <summary>
        /// Gets all comments for a post.
        /// </summary>
        /// <param name="postId">Post ID.</param>
        /// <param name="paginationParams">Parameters for pagination.</param>
        /// <returns>A list of all comments from a post.</returns>
        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostComments(int postId, [FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var request = new GetPostCommentsQuery { 
                Params = paginationParams,
                PostId = postId,
                AuthUserId = AuthService.GetCurrentUserId(),
                AuthUserRoles = AuthService.GetCurrentUserRoles()
            };

            var response = await Mediator.Send(request, cancellationToken);

            Response.AddPaginationHeader(new PaginationHeader(response.Comments.CurrentPage, response.Comments.PageSize, response.Comments.TotalCount, response.Comments.TotalPages));

            return Ok(response);
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
            var request = new UploadCommentCommand
            {
                Comment = commentUploadDto,
                AuthUserId = AuthService.GetCurrentUserId()
            };

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
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
            var request = new DeleteCommentCommand
            {
                CommentId = commentId,
                AuthUserId = AuthService.GetCurrentUserId(),
                AuthUserRoles = AuthService.GetCurrentUserRoles()
            };

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Returns a list of users who liked a comment.
        /// </summary>
        /// <param name="commentId">Comment ID.</param>
        /// <returns>List of user DTOs.</returns>
        [HttpGet("{commentId}/likes")]
        public async Task<IActionResult> GetCommentLikes(int commentId, CancellationToken cancellationToken)
        {
            var request = new GetCommentLikesQuery 
            { 
                CommentId = commentId,
                AuthUserId = AuthService.GetCurrentUserId() 
            };

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Adds a like to a comment.
        /// </summary>
        /// <param name="commentId">Comment ID.</param>
        /// <returns>Like confirmation.</returns>
        [HttpPost("{commentId}/like")]
        public async Task<IActionResult> LikeComment(int commentId, CancellationToken cancellationToken)
        {
            var request = new LikeCommentCommand { 
                CommentId = commentId,
                AuthUserId = AuthService.GetCurrentUserId()
            };

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Removes a like from a comment.
        /// </summary>
        /// <param name="commentId">Comment ID.</param>
        /// <returns>Confirmation of the unlike.</returns>
        [HttpDelete("{commentId}/like")]
        public async Task<IActionResult> UnlikeComment(int commentId, CancellationToken cancellationToken)
        {
            var request = new UnlikeCommentCommand
            {
                CommentId = commentId,
                AuthUserId = AuthService.GetCurrentUserId()
            };

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
        }
    }
}
