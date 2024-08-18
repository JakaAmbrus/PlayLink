using Application.Features.MessageGroups.AddConnectionToGroup;
using Application.Features.MessageGroups.AddGroup;
using Application.Features.MessageGroups.Common;
using Application.Features.MessageGroups.GetGroupForConnection;
using Application.Features.MessageGroups.GetMessageGroup;
using Application.Features.Messages.MarkMessageAsRead;
using Application.Features.MessageGroups.RemoveConnection;
using Application.Features.Messages.Common;
using Application.Features.Messages.GetMessageById;
using Application.Features.Messages.GetMessageThread;
using Application.Features.Messages.SendMessage;
using Application.Features.Users.GetUserIdFromUsername;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace WebAPI.SignalR
{
    [Authorize]
    public class MessageHub : Hub
    {
        private readonly ISender _mediator;
        private readonly IAuthService _authService;
        private readonly IHubContext<PresenceHub> _presenceHub;

        private static readonly ConcurrentDictionary<int, Queue<DateTime>> MessageTime = new();

        public MessageHub(ISender mediator, IAuthService authService, IHubContext<PresenceHub> presenceHub)
        {
            _mediator = mediator;
            _authService = authService;
            _presenceHub = presenceHub;
        }

        public override async Task OnConnectedAsync()
        {
            var authUserId = _authService.GetCurrentUserId();
            var authUsername = await _authService.GetUsernameByIdAsync(CancellationToken.None);

            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["user"].ToString();

            var groupName = GetGroupName(authUsername, otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var group = await AddToGroup(groupName);

            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);


            var query = new GetMessageThreadQuery
            {
                AuthUserId = authUserId,
                ProfileUsername = otherUser
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

            int authUserId = _authService.GetCurrentUserId();

            if (!IsWithinMessageRateLimit(authUserId, out var retryAfter))
            {
                var waitTime = retryAfter.HasValue ? (int)retryAfter.Value.TotalSeconds : 0;

                await Clients.Caller.SendAsync("TooManyMessages", $"Too many messages sent, please wait {waitTime}s");

                return;
            }

            var authUsername = await _authService.GetUsernameByIdAsync(CancellationToken.None);;

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
            else
            {
                int recipientId = await _mediator.Send(new GetUserIdByUsernameQuery { Username = result.Message.RecipientUsername });

                var connections = await PresenceTracker.GetConnectionsForUser(recipientId);
                if (connections != null)
                {
                    await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived", new
                    {
                        username = authUsername,
                        fullName = result.Message.SenderFullName,
                    });
                }
            }
            await Clients.Group(groupName).SendAsync("NewMessage", result.Message);
        }

        private static bool IsWithinMessageRateLimit(int userId, out TimeSpan? retryAfter)
        {
            var currentTime = DateTime.UtcNow;
            var timestamps = MessageTime.GetOrAdd(userId, new Queue<DateTime>());

            lock (timestamps)
            {
                while (timestamps.Any() && (currentTime - timestamps.Peek()).TotalMinutes > 1)
                {
                    timestamps.Dequeue();
                }

                if (timestamps.Count >= 5)
                {
                    var oldestTimestamp = timestamps.Peek();
                    retryAfter = TimeSpan.FromMinutes(1) - (currentTime - oldestTimestamp);
                    return false;
                }

                timestamps.Enqueue(currentTime);
                retryAfter = null;
                return true;
            }
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

            string authUsername = await _authService.GetUsernameByIdAsync(CancellationToken.None);
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
