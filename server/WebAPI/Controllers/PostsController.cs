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
  
    public class PostsController : BaseAuthApiController
    {

        public PostsController(ISender mediator, IAuthenticatedUserService authenticatedUserService) : base(mediator, authenticatedUserService)
        {
        }

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

        [HttpGet("user/{username}/photos")]
        public async Task<IActionResult> GetUserPostPhotos(string username, CancellationToken cancellationToken)
        {
            var query = new GetUserPostPhotosQuery { Username = username };

            var result = await Mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<UploadPostResponse>> UploadPost([FromForm] PostContentDto postContent, CancellationToken cancellationToken)
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

        [HttpDelete("{postId}")]
        public async Task<ActionResult<Unit>> DeletePost(int postId, CancellationToken cancellationToken)
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
