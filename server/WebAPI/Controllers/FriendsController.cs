using Application.Features.Friends.Common;
using Application.Features.Friends.GetFriendRequests;
using Application.Features.Friends.GetRelationshipStatus;
using Application.Features.Friends.GetUserFriends;
using Application.Features.Friends.RemoveFriendRequest;
using Application.Features.Friends.RemoveFriendship;
using Application.Features.Friends.RespondToFriendRequest;
using Application.Features.Friends.SendFriendRequest;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Handles friend requests and friendships.
    /// </summary>
    public class FriendsController : BaseController
    {
        /// <summary>
        /// Fetches friends of the currently authenticated user.
        /// </summary>
        /// <returns>A list of Friend User DTOs</returns>
        [HttpGet]
        public async Task<IActionResult> GetUserFriends(CancellationToken cancellationToken)
        {
            var request = new GetUserFriendsQuery
            {
                AuthUserId = AuthService.GetCurrentUserId()
            };

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Gets all friend requests of the currently authenticated user.
        /// </summary>
        /// <returns>A list of Friend Request DTOs</returns>
        [HttpGet("requests")]
        public async Task<IActionResult> GetFriendRequests(CancellationToken cancellationToken)
        {
            var request = new GetFriendRequestsQuery
            {
                AuthUserId = AuthService.GetCurrentUserId()
            };

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Gets the friendship status between the currently authenticated user and the user with the given username.
        /// </summary>
        /// <param name="username">Username of the profile user.</param>
        /// <returns>The friendship status.</returns>
        [HttpGet("status/{username}")]
        public async Task<IActionResult> GetFriendshipStatus(string username, CancellationToken cancellationToken)
        {
            var request = new GetFriendshipStatusQuery
            {
                ProfileUsername = username,
                AuthUserId = AuthService.GetCurrentUserId()
            };

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Sends a friend request to the user with the given username.
        /// </summary>
        /// <param name="receiverUsername">Username of the profile user.</param>
        /// <returns>Confirmation of the sent request.</returns>
        [HttpPost("{receiverUsername}")]
        public async Task<IActionResult> SendFriendRequest(string receiverUsername, CancellationToken cancellationToken)
        {
            var request = new SendFriendRequestCommand
            {
                ReceiverUsername = receiverUsername,
                AuthUserId = AuthService.GetCurrentUserId()
            };

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Confirms or rejects a friend request.
        /// </summary>
        /// <param name="friendRequestResponse">Confirmation or rejection of a friend request.</param>
        /// <returns>Boolean value if the friend request was rejected or accepted, if accepted it returns the user DTO.</returns>
        [HttpPut("request-response")]
        public async Task<IActionResult> RespondToFriendRequest(FriendRequestResponseDto friendRequestResponse, CancellationToken cancellationToken)
        {
            var request = new RespondToFriendRequestCommand
            {
                FriendRequestResponse = friendRequestResponse,
                AuthUserId = AuthService.GetCurrentUserId()
            };

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Removes a friendship between the currently authenticated user and the user with the given username.
        /// </summary>
        /// <param name="username">Username of the profile user.</param>
        /// <returns>Confirmation of successful removal.</returns>
        [HttpDelete("{username}")]
        public async Task<IActionResult> RemoveFriendship(string username, CancellationToken cancellationToken)
        {
            var request = new RemoveFriendshipCommand
            {
                ProfileUsername = username,
                AuthUserId = AuthService.GetCurrentUserId()
            };

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Removes a friend request.
        /// </summary>
        /// <param name="friendRequestId">Id of the friend request.</param>
        /// <returns>Confirmation of successful removal.</returns>
        [HttpDelete("request/{friendRequestId}")]
        public async Task<IActionResult> RemoveFriendRequest(int friendRequestId, CancellationToken cancellationToken)
        {
            var request = new RemoveFriendRequestCommand
            {
                FriendRequestId = friendRequestId,
                AuthUserId = AuthService.GetCurrentUserId()
            };

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
        }
    }
}
