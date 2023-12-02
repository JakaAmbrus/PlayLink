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

    public class MessagesController : BaseAuthApiController
    {
        public MessagesController(ISender mediator, IAuthenticatedUserService authenticatedUserService) : base(mediator, authenticatedUserService)
        {
        }

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
