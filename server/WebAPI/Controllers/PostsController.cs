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
using Application.Utils;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Extensions;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Manages posts related operations.
    /// </summary>
    public class PostsController : BaseController
    {
        /// <summary>
        /// Returns a list of posts from the Database.
        /// </summary>
        /// <param name="paginationParams">Parameters for pagination</param>
        /// <returns>A list of Post DTOs representing all available posts and confirms if the user is authorized to perform certain critical actions tot he post.</returns>
        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var request = new GetPostsQuery {
                Params = paginationParams,
                AuthUserId = AuthService.GetCurrentUserId(),
                AuthUserRoles = AuthService.GetCurrentUserRoles()
            };

            var response = await Mediator.Send(request, cancellationToken);
            Response.AddPaginationHeader(new PaginationHeader(response.Posts.CurrentPage, response.Posts.PageSize, response.Posts.TotalCount, response.Posts.TotalPages));
            return Ok(response);
        }

        /// <summary>
        /// Returns a single post from the Database from the provided ID.
        /// </summary>
        /// <param name="postId">ID of the Post</param>
        /// <returns>A Post DTO with confirmation of authorization for the current user fetching the post</returns>
        [HttpGet("{postId:int}")]
        public async Task<IActionResult> GetPostById(int postId, CancellationToken cancellationToken)
        {
            var request = new GetPostByIdQuery { 
                PostId = postId,
                AuthUserId = AuthService.GetCurrentUserId(),
                AuthUserRoles = AuthService.GetCurrentUserRoles()
            };

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
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
            var request = new GetPostsByUserQuery 
            {
                Username = username,
                Params = paginationParams,
                AuthUserId = AuthService.GetCurrentUserId(),
                AuthUserRoles = AuthService.GetCurrentUserRoles()
            };

            var response = await Mediator.Send(request, cancellationToken);
            Response.AddPaginationHeader(new PaginationHeader(response.Posts.CurrentPage, response.Posts.PageSize, response.Posts.TotalCount, response.Posts.TotalPages));
            return Ok(response);
        }

        /// <summary>
        /// Returns a list of photos from the Database for the user with the provided username.
        /// </summary>
        /// <param name="username">User Username.</param>
        /// <returns>A list of photo URLs from users posts and his profile photo.</returns>
        [HttpGet("user/{username}/photos")]
        public async Task<IActionResult> GetUserPostPhotos(string username, CancellationToken cancellationToken)
        {
            var request = new GetUserPostPhotosQuery { Username = username };

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Creates a new post and adds it to the Database and if there is a photo, adds it to cloudinary.
        /// </summary>
        /// <param name="postContent">PostContentDTO with the text content of the post and the photo file.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadPost([FromForm] PostContentDto postContent, CancellationToken cancellationToken)
        {
            var request = new UploadPostCommand
            {
                PostContentDto = postContent,
                AuthUserId = AuthService.GetCurrentUserId()
            };

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
        }


        /// <summary>
        /// Deletes a post from the Database and if there is a photo, deletes it from cloudinary.
        /// </summary>
        /// <param name="postId">Post ID.</param>
        /// <returns></returns>
        [HttpDelete("{postId}")]
        public async Task<IActionResult> DeletePost(int postId, CancellationToken cancellationToken)
        {
            var request = new DeletePostCommand 
            { 
                PostId = postId,
                AuthUserId = AuthService.GetCurrentUserId(),
                AuthUserRoles = AuthService.GetCurrentUserRoles()
            };

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Returns a list of users who liked a post.
        /// </summary>
        /// <param name="postId">Post ID.</param>
        /// <returns>List of user DTOs.</returns>
        [HttpGet("{postId:int}/likes")]
        public async Task<IActionResult> GetPostLikes(int postId, CancellationToken cancellationToken)
        {
            var request = new GetPostLikesQuery 
            { 
                PostId = postId,
                AuthUserId = AuthService.GetCurrentUserId()
            };

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Adds a like to a post.
        /// </summary>
        /// <param name="postId">Post ID.</param>
        /// <returns>Like confirmation.</returns>
        [HttpPost("{postId:int}/like")]
        public async Task<IActionResult> LikePost(int postId, CancellationToken cancellationToken)
        {
            var request = new LikePostCommand
            {
                PostId = postId,
                AuthUserId = AuthService.GetCurrentUserId()
            };

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Removes a like from a post.
        /// </summary>
        /// <param name="postId">Post ID</param>
        /// <returns>Confirmation of the unlike.</returns>
        [HttpDelete("{postId:int}/like")]
        public async Task<IActionResult> UnlikePost(int postId, CancellationToken cancellationToken)
        {
            var request = new UnlikePostCommand 
            { 
                PostId = postId,
                AuthUserId = AuthService.GetCurrentUserId()
            };

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
        }
    }
}
