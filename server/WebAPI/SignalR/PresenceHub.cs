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

            await _tracker.UserConnected(authUserId, Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOnline", authUserId);

            string[] currentUsers = await _tracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            int authUserId = _authenticatedUserService.UserId;

            await _tracker.UserDisconnected(authUserId, Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOffline", authUserId);

            string[] currentUsers = await _tracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
