using Application.Features.Likes.GetPostLikes;
using Application.Features.Likes.LikePost;
using Application.Features.Likes.UnlikePost;
using Application.Features.Posts.Common;
using Application.Features.Posts.DeletePost;
using Application.Features.Posts.GetPostById;
using Application.Features.Posts.GetPosts;
using Application.Features.Posts.GetPostsByUser;
using Application.Features.Posts.GetUserPostPhotos;
using Application.Features.Posts.UploadPost;
using Application.Interfaces;
using Application.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Extensions;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Manages posts related operations.
    /// </summary>
    public class PostsController : BaseAuthApiController
    {

        public PostsController(ISender mediator, IAuthenticatedUserService authenticatedUserService) : base(mediator, authenticatedUserService)
        {
        }

        /// <summary>
        /// Returns a list of posts from the Database.
        /// </summary>
        /// <param name="paginationParams">Parameters for pagination</param>
        /// <returns>A list of Post DTOs representing all available posts and confirms if the user is authorized to perform certain critical actions tot he post.</returns>
        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();
            IEnumerable<string> authUserRoles = GetCurrentUserRoles();

            var query = new GetPostsQuery {
                Params = paginationParams,
                AuthUserId = authUserId,
                AuthUserRoles = authUserRoles
            };

            var posts = await Mediator.Send(query, cancellationToken);

            Response.AddPaginationHeader(new PaginationHeader(posts.Posts.CurrentPage, posts.Posts.PageSize, posts.Posts.TotalCount, posts.Posts.TotalPages));

            return Ok(posts);
        }

        /// <summary>
        /// Returns a single post from the Database from the provided ID.
        /// </summary>
        /// <param name="postId">ID of the Post</param>
        /// <returns>A Post DTO with confirmation of authorization for the current user fetching the post</returns>
        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostById(int postId, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();
            IEnumerable<string> authUserRoles = GetCurrentUserRoles();

            var query = new GetPostByIdQuery { 
                PostId = postId,
                AuthUserId = authUserId,
                AuthUserRoles = authUserRoles
            };

            var result = await Mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Returns a list of posts from the Database for the user with the provided username.
        /// </summary>
        /// <param name="username">Username of the user whose posts.</param>
        /// <param name="paginationParams"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("user/{username}")]
        public async Task<IActionResult> GetPostsByUser(string username, [FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();
            IEnumerable<string> authUserRoles = GetCurrentUserRoles();

            var query = new GetPostsByUserQuery 
            {
                Username = username,
                Params = paginationParams,
                AuthUserId = authUserId,
                AuthUserRoles = authUserRoles
            };

            var result = await Mediator.Send(query, cancellationToken);

            Response.AddPaginationHeader(new PaginationHeader(result.Posts.CurrentPage, result.Posts.PageSize, result.Posts.TotalCount, result.Posts.TotalPages));

            return Ok(result);
        }

        /// <summary>
        /// Returns a list of photos from the Database for the user with the provided username.
        /// </summary>
        /// <param name="username">User Username.</param>
        /// <returns>A list of photo URLs from users posts and his profile photo.</returns>
        [HttpGet("user/{username}/photos")]
        public async Task<IActionResult> GetUserPostPhotos(string username, CancellationToken cancellationToken)
        {
            var query = new GetUserPostPhotosQuery { Username = username };

            var result = await Mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Creates a new post and adds it to the Database and if there is a photo, adds it to cloudinary.
        /// </summary>
        /// <param name="postContent">PostContentDTO with the text content of the post and the photo file.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadPost([FromForm] PostContentDto postContent, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();

            var command = new UploadPostCommand
            {
                PostContentDto = postContent,
                AuthUserId = authUserId, 
            };

            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }


        /// <summary>
        /// Deletes a post from the Database and if there is a photo, deletes it from cloudinary.
        /// </summary>
        /// <param name="postId">Post ID.</param>
        /// <returns></returns>
        [HttpDelete("{postId}")]
        public async Task<IActionResult> DeletePost(int postId, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();
            IEnumerable<string> authUserRoles = GetCurrentUserRoles();

            var command = new DeletePostCommand 
            { 
                PostId = postId,
                AuthUserId = authUserId,
                AuthUserRoles = authUserRoles
            };

            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Returns a list of users who liked a post.
        /// </summary>
        /// <param name="postId">Post ID.</param>
        /// <returns>List of user DTOs.</returns>
        [HttpGet("{postId}/likes")]
        public async Task<IActionResult> GetPostLikes(int postId, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();

            var query = new GetPostLikesQuery 
            { 
                PostId = postId,
                AuthUserId = authUserId
            };

            var result = await Mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Adds a like to a post.
        /// </summary>
        /// <param name="commentId">Post ID.</param>
        /// <returns>Like confirmation.</returns>
        [HttpPost("{postId}/like")]
        public async Task<IActionResult> LikePost(int postId, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();

            var command = new LikePostCommand
            {
                PostId = postId,
                AuthUserId = authUserId
            };

            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Removes a like from a post.
        /// </summary>
        /// <param name="commentId">Post ID.</param>
        /// <returns>Confirmation of the unlike.</returns>
        [HttpDelete("{postId}/like")]
        public async Task<IActionResult> UnlikePost(int postId, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();

            var command = new UnlikePostCommand 
            { 
                PostId = postId,
                AuthUserId = authUserId    
            };

            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }
    }
}
