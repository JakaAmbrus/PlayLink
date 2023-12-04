using Application.Features.Messages.Common;
using Application.Features.Messages.GetMessageThread;
using Application.Features.Messages.SendMessage;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebAPI.SignalR
{
    [Authorize]
    public class MessageHub : Hub
    {
        private readonly ISender _mediator;
        private readonly IAuthenticatedUserUsernameService _authenticatedUserUsernameService;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public MessageHub(ISender mediator, IAuthenticatedUserService authenticatedUserService , IAuthenticatedUserUsernameService authenticatedUserUsernameService)
        {
            _mediator = mediator;
            _authenticatedUserUsernameService = authenticatedUserUsernameService;
            _authenticatedUserService = authenticatedUserService;
        }

        public override async Task OnConnectedAsync()
        {
            int authUserId = _authenticatedUserService.UserId;
            string authUsername = await _authenticatedUserUsernameService.GetUsernameByIdAsync();

            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["user"].ToString();

            var groupName = GetGroupName(authUsername, otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var query = new GetMessageThreadQuery
            {
                AuthUserId = authUserId,
                RecipientUsername = otherUser
            };
            var messages = await _mediator.Send(query);

            await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            int authUserId = _authenticatedUserService.UserId;

            var command = new SendMessageCommand
            {
                CreateMessageDto = createMessageDto,
                AuthUserId = authUserId
            };

            var result = await _mediator.Send(command);

            var group = GetGroupName(result.Message.SenderUsername, result.Message.RecipientUsername);

            await Clients.Group(group).SendAsync("NewMessage", result.Message);
        }

        private static string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }    
    }
}
