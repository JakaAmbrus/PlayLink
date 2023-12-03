using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebAPI.SignalR
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly PresenceTracker _tracker;

        public PresenceHub(IAuthenticatedUserService authenticatedUserService, PresenceTracker tracker)
        {
            _authenticatedUserService = authenticatedUserService;
            _tracker = tracker;
        }

        public override async Task OnConnectedAsync()
        {
            int authUserId = _authenticatedUserService.UserId;

            var isOnline = await _tracker.UserConnected(authUserId, Context.ConnectionId);
            if (isOnline)
            {
                await Clients.Others.SendAsync("UserIsOnline", authUserId);
            }

            var currentUsers = await _tracker.GetOnlineUsers();
            await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            int authUserId = _authenticatedUserService.UserId;

            var isOffline = await _tracker.UserDisconnected(authUserId, Context.ConnectionId);
            if (isOffline)
            {
                await Clients.Others.SendAsync("UserIsOffline", authUserId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
