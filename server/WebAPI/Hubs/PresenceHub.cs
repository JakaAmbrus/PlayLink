using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebAPI.Hubs
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public PresenceHub(IAuthenticatedUserService authenticatedUserService)
        {
            _authenticatedUserService = authenticatedUserService;
        }

        public override async Task OnConnectedAsync()
        {
            int authUserId = _authenticatedUserService.UserId;

            await Clients.Others.SendAsync("UserIsOnline", authUserId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            int authUserId = _authenticatedUserService.UserId;

            await Clients.Others.SendAsync("UserIsOffline", authUserId);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
