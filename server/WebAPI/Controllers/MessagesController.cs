using Application.Features.Messages.Common;
using Application.Features.Messages.GetMessagesForUser;
using Application.Features.Messages.SendMessage;
using Application.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Extensions;

namespace WebAPI.Controllers
{

    public class MessagesController : BaseApiController
    {
        private readonly IMediator _mediator;

        public MessagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("user")]
        public async Task<ActionResult<PagedList<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams, CancellationToken cancellationToken)
        {
            var query = new GetMessagesForUserQuery
            {
                Params = messageParams
            };

            var result = await _mediator.Send(query, cancellationToken);

            Response.AddPaginationHeader(new PaginationHeader(result.Messages.CurrentPage, result.Messages.PageSize, result.Messages.TotalCount, result.Messages.TotalPages));

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto, CancellationToken cancellationToken)
        {
            var command = new SendMessageCommand { CreateMessageDto = createMessageDto };

            var result = await _mediator.Send(command, cancellationToken);

            return Ok(result);
        }

    }
}
