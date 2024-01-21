using Application.Features.Friends.Common;
using Application.Features.Friends.GetFriendRequests;
using Application.Features.Friends.GetRelationshipStatus;
using Application.Features.Friends.GetUserFriends;
using Application.Features.Friends.RemoveFriendRequest;
using Application.Features.Friends.RemoveFriendship;
using Application.Features.Friends.RespondToFriendRequest;
using Application.Features.Friends.SendFriendRequest;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Handles friend requests and friendships.
    /// </summary>
    public class FriendsController : BaseAuthApiController
    {
        public FriendsController(ISender mediator, IAuthenticatedUserService authenticatedUserService) : base(mediator, authenticatedUserService)
        {
        }

        /// <summary>
        /// Fetches friends of the currently authenticated user.
        /// </summary>
        /// <returns>A list of Friend User DTOs</returns>
        [HttpGet]
        public async Task<IActionResult> GetUserFriends(CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();

            var query = new GetUserFriendsQuery
            {
                AuthUserId = authUserId
            };

            var result = await Mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Gets all friend requests of the currently authenticated user.
        /// </summary>
        /// <returns>A list of Friend Request DTOs</returns>
        [HttpGet("requests")]
        public async Task<IActionResult> GetFriendRequests(CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();

            var query = new GetFriendRequestsQuery
            {
                AuthUserId = authUserId
            };

            var result = await Mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Gets the friendship status between the currently authenticated user and the user with the given username.
        /// </summary>
        /// <param name="username">Username of the profile user.</param>
        /// <returns>The friendship status.</returns>
        [HttpGet("status/{username}")]
        public async Task<IActionResult> GetFriendshipStatus(string username, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();

            var query = new GetFriendshipStatusQuery
            {
                ProfileUsername = username,
                AuthUserId = authUserId
            };

            var result = await Mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Sends a friend request to the user with the given username.
        /// </summary>
        /// <param name="receiverUsername">Username of the profile user.</param>
        /// <returns>Confirmation of the sent request.</returns>
        [HttpPost("{receiverUsername}")]
        public async Task<IActionResult> SendFriendRequest(string receiverUsername, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();

            var command = new SendFriendRequestCommand
            {
                ReceiverUsername = receiverUsername,
                AuthUserId = authUserId
            };

            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Confirms or rejects a friend request.
        /// </summary>
        /// <param name="friendRequestResponse">Confirmation or rejection of a friend request.</param>
        /// <returns>Boolean value if the friend request was rejected or accepted, if accepted it returns the user DTO.</returns>
        [HttpPut("request-response")]
        public async Task<IActionResult> RespondToFriendRequest(FriendRequestResponseDto friendRequestResponse, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();

            var command = new RespondToFriendRequestCommand
            {
                FriendRequestResponse = friendRequestResponse,
                AuthUserId = authUserId
            };

            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Removes a friendship between the currently authenticated user and the user with the given username.
        /// </summary>
        /// <param name="username">Username of the profile user.</param>
        /// <returns>Confirmation of successful removal.</returns>
        [HttpDelete("{username}")]
        public async Task<IActionResult> RemoveFriendship(string username, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();

            var command = new RemoveFriendshipCommand
            {
                ProfileUsername = username,
                AuthUserId = authUserId
            };

            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Removes a friend request.
        /// </summary>
        /// <param name="friendRequestId">Id of the friend request.</param>
        /// <returns>Confirmation of successful removal.</returns>
        [HttpDelete("request/{friendRequestId}")]
        public async Task<IActionResult> RemoveFriendRequest(int friendRequestId, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();

            var command = new RemoveFriendRequestCommand
            {
                FriendRequestId = friendRequestId,
                AuthUserId = authUserId
            };

            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }
    }
}
