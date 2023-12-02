using Application.Features.Messages.Common;
using Application.Features.Messages.DeleteMessage;
using Application.Features.Messages.GetMessagesForUser;
using Application.Features.Messages.GetMessageThread;
using Application.Features.Messages.SendMessage;
using Application.Interfaces;
using Application.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Extensions;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Manages messages related operations.
    /// </summary>
    public class MessagesController : BaseAuthApiController
    {
        public MessagesController(ISender mediator, IAuthenticatedUserService authenticatedUserService) : base(mediator, authenticatedUserService)
        {
        }

        /// <summary>
        /// Returns all messages related to a user based on filters: Inbox, Outbox, Unread.
        /// </summary>
        /// <param name="messageParams">Parameters for filtering and pagination.</param>
        /// <returns>A paginated list of messages for a user based on the filters.</returns>
        [HttpGet("user")]
        public async Task<ActionResult<PagedList<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();

            var query = new GetMessagesForUserQuery
            {
                Params = messageParams,
                AuthUserId = authUserId
            };

            var result = await Mediator.Send(query, cancellationToken);

            Response.AddPaginationHeader(new PaginationHeader(result.Messages.CurrentPage, result.Messages.PageSize, result.Messages.TotalCount, result.Messages.TotalPages));

            return Ok(result);
        }

        /// <summary>
        /// Returns a list of messages between two users.
        /// </summary>
        /// <param name="recipientUsername">Username of the receiving user.</param>
        /// <returns>Message DTOs of the message thread between two users.</returns>
        [HttpGet("thread/{recipientUsername}")]
        public async Task<ActionResult<List<MessageDto>>> GetMessageThread(string recipientUsername, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();

            var query = new GetMessageThreadQuery
            {
                RecipientUsername = recipientUsername,
                AuthUserId = authUserId
            };

            var result = await Mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Sends a message to a user.
        /// </summary>
        /// <param name="createMessageDto">DTO containing the message and the username of the recipient.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();

            var command = new SendMessageCommand 
            { 
                CreateMessageDto = createMessageDto,
                AuthUserId = authUserId
            };

            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Deletes a message.
        /// </summary>
        /// <param name="id">Message ID.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id, CancellationToken cancellationToken)
        {
            int authUserId = GetCurrentUserId();

            var command = new DeleteMessageCommand 
            { 
                PrivateMessageId = id,
                AuthUserId = authUserId,
            };

            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }

    }
}
