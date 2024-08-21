using Social.Application.Features.Messages.Common;
using Social.Application.Features.Messages.DeleteMessage;
using Social.Application.Features.Messages.GetMessagesForUser;
using Social.Application.Features.Messages.GetMessageThread;
using Social.Application.Features.Messages.SendMessage;
using Social.Application.Utils;
using Microsoft.AspNetCore.Mvc;
using Social.Api.Extensions;

namespace Social.Api.Controllers
{
    /// <summary>
    /// Manages messages related operations.
    /// </summary>
    public class MessagesController : BaseController
    {
        /// <summary>
        /// Returns all messages related to a user based on filters: Inbox, Outbox, Unread.
        /// </summary>
        /// <param name="messageParams">Parameters for filtering and pagination.</param>
        /// <returns>A paginated list of messages for a user based on the filters.</returns>
        [HttpGet("user")]
        public async Task<IActionResult> GetMessagesForUser([FromQuery] MessageParams messageParams, CancellationToken cancellationToken)
        {
            var request = new GetMessagesForUserQuery
            {
                Params = messageParams,
                AuthUserId = AuthService.GetCurrentUserId()
            };

            var response = await Mediator.Send(request, cancellationToken);
            Response.AddPaginationHeader(new PaginationHeader(response.Messages.CurrentPage, response.Messages.PageSize, response.Messages.TotalCount, response.Messages.TotalPages));
            return Ok(response);
        }

        /// <summary>
        /// Returns a list of messages between two users.
        /// </summary>
        /// <param name="recipientUsername">Username of the receiving user.</param>
        /// <returns>Message DTOs of the message thread between two users.</returns>
        [HttpGet("thread/{recipientUsername}")]
        public async Task<IActionResult> GetMessageThread(string recipientUsername, CancellationToken cancellationToken)
        {
            var request = new GetMessageThreadQuery
            {
                ProfileUsername = recipientUsername,
                AuthUserId = AuthService.GetCurrentUserId()
            };

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Sends a message to a user.
        /// </summary>
        /// <param name="createMessageDto">DTO containing the message and the username of the recipient.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateMessage(CreateMessageDto createMessageDto, CancellationToken cancellationToken)
        {
            var request = new SendMessageCommand 
            { 
                CreateMessageDto = createMessageDto,
                AuthUserId = AuthService.GetCurrentUserId()
            };

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Deletes a message.
        /// </summary>
        /// <param name="id">Message ID.</param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMessage(int id, CancellationToken cancellationToken)
        {
            var request = new DeleteMessageCommand 
            { 
                PrivateMessageId = id,
                AuthUserId = AuthService.GetCurrentUserId()
            };

            var response = await Mediator.Send(request, cancellationToken);
            return Ok(response);
        }

    }
}
