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
    public class FriendsController : BaseAuthApiController
    {
        public FriendsController(ISender mediator, IAuthenticatedUserService authenticatedUserService) : base(mediator, authenticatedUserService)
        {
        }

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
