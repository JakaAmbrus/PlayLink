using Application.Features.MessageGroups.AddConnectionToGroup;
using Application.Features.MessageGroups.AddGroup;
using Application.Features.MessageGroups.Common;
using Application.Features.MessageGroups.GetGroupForConnection;
using Application.Features.MessageGroups.GetMessageGroup;
using Application.Features.MessageGroups.MarkMessageAsRead;
using Application.Features.MessageGroups.RemoveConnection;
using Application.Features.Messages.Common;
using Application.Features.Messages.GetMessageById;
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

            var group = await AddToGroup(groupName);

            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);


            var query = new GetMessageThreadQuery
            {
                AuthUserId = authUserId,
                RecipientUsername = otherUser
            };
            var messages = await _mediator.Send(query);

            await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var group = await RemoveFromMessageGroup();
            await Clients.Group(group.Name).SendAsync("UpdatedGroup");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            int authUserId = _authenticatedUserService.UserId;
            string authUsername = await _authenticatedUserUsernameService.GetUsernameByIdAsync();

            var command = new SendMessageCommand
            {
                CreateMessageDto = createMessageDto,
                AuthUserId = authUserId
            };       

            var result = await _mediator.Send(command);

            var groupName = GetGroupName(result.Message.SenderUsername, result.Message.RecipientUsername);

            var groupDto = await _mediator.Send(new GetMessageGroupQuery { GroupName = groupName });

            if (groupDto != null && groupDto.Connections.Any(c => c.Username == result.Message.RecipientUsername))
            {
                await _mediator.Send(new MarkMessageAsReadCommand { MessageId = result.Message.PrivateMessageId });
                result.Message = await _mediator.Send(new GetMessageByIdQuery { MessageId = result.Message.PrivateMessageId });
            }

            await Clients.Group(groupName).SendAsync("NewMessage", result.Message);
        }

        private static string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }

        private async Task<GroupDto> AddToGroup(string groupName)
        {
            var groupDto = await _mediator.Send(new GetMessageGroupQuery { GroupName = groupName });

            if (groupDto == null)
            {
                _ = await _mediator.Send(new AddGroupCommand { GroupName = groupName });
            }

            string authUsername = await _authenticatedUserUsernameService.GetUsernameByIdAsync();
            var connectionDto = new ConnectionDto
            {
                ConnectionId = Context.ConnectionId,
                Username = authUsername
            };

            await _mediator.Send(new AddConnectionToGroupCommand { GroupName = groupName, ConnectionDto = connectionDto });

            groupDto = await _mediator.Send(new GetMessageGroupQuery { GroupName = groupName });

            return groupDto;
        }

        private async Task<GroupDto> RemoveFromMessageGroup()
        {
            var groupDto = await _mediator.Send(new GetGroupForConnectionQuery
            {
                ConnectionId = Context.ConnectionId
            });

            await _mediator.Send(new RemoveConnectionCommand
            {
                ConnectionId = Context.ConnectionId
            });

            return groupDto;
        }
    }
}
